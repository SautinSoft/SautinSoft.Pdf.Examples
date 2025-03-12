using System;
using System.IO;
using System.IO.Compression;
using SautinSoft.Pdf.Annotations;
using SautinSoft.Pdf;

class Program
{
    /// <summary>
    /// Annotations.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/extract-attachment-annotations.php
    /// </remarks>
    static void Main()
    {
        // Before starting this example, please get a free trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        // Add to zip archive all files from file attachment annotations located on the first page.
        using (var document = PdfDocument.Load(Path.GetFullPath(@"..\..\..\File Attachment Annotations.pdf")))
        using (var archiveStream = File.Create("File Attachment Annotation Files.zip"))
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
            foreach (var annotation in document.Pages[0].Annotations)
                if (annotation.AnnotationType == PdfAnnotationType.FileAttachment)
                {
                    var fileAttachmentAnnotation = (PdfFileAttachmentAnnotation)annotation;

                    var fileSpecification = fileAttachmentAnnotation.File;

                    // Use the description or the file name as the relative path of the entry in the zip archive.
                    var entryFullName = fileAttachmentAnnotation.Description;
                    if (entryFullName == null || !entryFullName.EndsWith(fileSpecification.Name, StringComparison.Ordinal))
                        entryFullName = fileSpecification.Name;

                    var embeddedFile = fileSpecification.EmbeddedFile;

                    // Create zip archive entry.
                    // Zip archive entry is compressed if the embedded file's compressed size is less than its uncompressed size.
                    bool compress = embeddedFile.Size == null || embeddedFile.CompressedSize < embeddedFile.Size.GetValueOrDefault();
                    var entry = archive.CreateEntry(entryFullName, compress ? CompressionLevel.Optimal : CompressionLevel.NoCompression);

                    // Set the modification date, if it is specified in the embedded file.
                    var modificationDate = embeddedFile.ModificationDate;
                    if (modificationDate != null)
                        entry.LastWriteTime = modificationDate.GetValueOrDefault();

                    // Copy embedded file contents to the zip archive entry.
                    using (var embeddedFileStream = embeddedFile.OpenRead())
                    using (var entryStream = entry.Open())
                        embeddedFileStream.CopyTo(entryStream);
                }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("File Attachment Annotation Files.zip") { UseShellExecute = true });
    }
}