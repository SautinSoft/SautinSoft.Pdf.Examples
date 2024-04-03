using System;
using System.IO;
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
            // Load a PDF document from a file.
            using (var document = PdfDocument.Load(pdfFile))
            {
                // Add a page.
                var page = document.Pages.Add();

                // Write a text.
                using (var formattedText = new PdfFormattedText())
                {
                    formattedText.Append("Hello World again!");

                    page.Content.DrawText(formattedText, new PdfPoint(100, 700));
                }

                // Save all the changes made to the current PDF document using an incremental update.
                document.Save();
            }
        }
    }
}
