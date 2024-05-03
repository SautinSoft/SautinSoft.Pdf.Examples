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
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/embed-files-from-stream-to-pdf-document.php
        /// </remarks>
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        using (var document = PdfDocument.Load(Path.GetFullPath(@"..\..\..\simple text.pdf")))
        {
            // Make Attachments panel visible.
            document.PageMode = PdfPageMode.UseAttachments;

            // Embed in the PDF document all the files from the zip archive.
            using (var archiveStream = File.OpenRead(@"..\..\..\Attachments.zip"))
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen: true))
                foreach (var entry in archive.Entries)
                    if (!string.IsNullOrEmpty(entry.Name))
                    {
                        var fileSpecification = document.EmbeddedFiles.AddEmpty(entry.Name).Value;

                        // Set embedded file description to the relative path of the file in the zip archive.
                        fileSpecification.Description = entry.FullName;

                        var embeddedFile = fileSpecification.EmbeddedFile;

                        // Set the embedded file size and modification date.
                        if (entry.Length < int.MaxValue)
                            embeddedFile.Size = (int)entry.Length;
                        embeddedFile.ModificationDate = entry.LastWriteTime;

                        // Copy embedded file contents from the zip archive entry.
                        // Embedded file is compressed if its compressed size in the zip archive is less than its uncompressed size.
                        using (var entryStream = entry.Open())
                        using (var embeddedFileStream = embeddedFile.OpenWrite(compress: entry.CompressedLength < entry.Length))
                            entryStream.CopyTo(embeddedFileStream);
                    }

            document.Save("Embedded Files from Streams.pdf");
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Embedded Files from Streams.pdf") { UseShellExecute = true });
    }
}