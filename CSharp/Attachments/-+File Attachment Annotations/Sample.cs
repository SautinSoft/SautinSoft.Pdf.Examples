using System;
using System.IO;
using System.IO.Compression;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Annotations;
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
                // Extract all the files in the zip archive to a directory on the file system.
                ZipFile.ExtractToDirectory(@"..\..\..\Attachments.zip", "Attachments");

                var page = document.Pages[0];
                int rowCount = 0;
                double spacing = page.CropBox.Width / 5,
                    left = spacing,
                    bottom = page.CropBox.Height - 200;

                // Add file attachment annotations to the PDF page from all the files extracted from the zip archive.
                foreach (var filePath in Directory.GetFiles("Attachments", "*", SearchOption.AllDirectories))
                {
                    var fileAttachmentAnnotation = page.Annotations.AddFileAttachment(left - 10, bottom - 10, filePath);

                    // Set a different icon for each file attachment annotation in a row.
                    fileAttachmentAnnotation.Appearance.Icon = (PdfFileAttachmentIcon)(rowCount + 1);

                    // Set attachment description to the relative path of the file in the zip archive.
                    fileAttachmentAnnotation.Description = filePath.Substring(filePath.IndexOf('\\') + 1).Replace('\\', '/');

                    // There are, at most, 4 file attachment annotations in a row.
                    ++rowCount;
                    if (rowCount < 4)
                        left += spacing;
                    else
                    {
                        rowCount = 0;
                        left = spacing;
                        bottom -= spacing;
                    }
                }

                // Delete the directory where zip archive files were extracted to.
                Directory.Delete("Attachments", recursive: true);

                document.Save("File Attachment Annotations from file system.pdf");
            }
        }
    }
}
