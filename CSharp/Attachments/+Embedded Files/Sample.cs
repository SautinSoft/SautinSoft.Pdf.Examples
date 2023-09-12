using System;
using System.IO;
using System.IO.Compression;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        static void Main(string[] args)
        {
            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                // Make Attachments panel visible.
                document.PageMode = PdfPageMode.UseAttachments;

                // Extract all the files in the zip archive to a directory on the file system.
                ZipFile.ExtractToDirectory(@"..\..\..\Attachments.zip", "Attachments");

                // Embed in the PDF document all the files extracted from the zip archive.
                foreach (var filePath in Directory.GetFiles("Attachments", "*", SearchOption.AllDirectories))
                {
                    var fileSpecification = document.EmbeddedFiles.Add(filePath).Value;

                    // Set embedded file description to the relative path of the file in the zip archive.
                    fileSpecification.Description = filePath.Substring(filePath.IndexOf('\\') + 1).Replace('\\', '/');
                }

                // Delete the directory where zip archive files were extracted to.
                Directory.Delete("Attachments", recursive: true);

                document.Save("Embedded Files from file system.pdf");
            }
        }
    }
}
