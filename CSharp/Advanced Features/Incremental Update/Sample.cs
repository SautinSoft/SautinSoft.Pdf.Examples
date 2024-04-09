using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Edit PDF files using incremental updates.
        /// </summary>
        /// <remarks>
        /// Details: http://sautinsoft/products/pdf/help/net/developer-guide/incremental-update.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

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
