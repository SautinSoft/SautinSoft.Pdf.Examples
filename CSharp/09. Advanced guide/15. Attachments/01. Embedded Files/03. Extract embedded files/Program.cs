using System;
using System.IO;
using System.IO.Compression;
using SautinSoft.Pdf;

class Program
{
    static void Main()
    {
        /// <summary>
        /// Embed files to PDF document.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/extract-embedded-files.php
        /// </remarks>
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        // Add to zip archive all files embedded in the PDF document.
        using (var document = PdfDocument.Load(Path.GetFullPath(@"..\..\..\Embedded Files.pdf")))
        using (var archiveStream = File.Create("Embedded Files.zip"))
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
            foreach (var keyFilePair in document.EmbeddedFiles)
            {
                var fileSpecification = keyFilePair.Value;

                // Use the description or the name as the relative path of the entry in the zip archive.
                var entryFullName = fileSpecification.Description;
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

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Embedded Files.zip") { UseShellExecute = true });
    }
}