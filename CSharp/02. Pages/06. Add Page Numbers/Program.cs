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
        /// Add page numbers to a PDF document in C# and .NET
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/add-page-numbers-to-a-pdf-document-in-csharp-dotnet.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string inpFile = Path.GetFullPath(@"..\..\..\Wine turism.pdf");
            string outFile = Path.GetFullPath("Result.pdf");

            using (PdfDocument document = PdfDocument.Load(inpFile))
            {
                // Iterate by all pages
                int pageNum = 1;
                foreach (var page in document.Pages)
                {
                    // Draw the page numbering atop of all.
                    // The method PdfContent(PdfContentGroup).DrawText always adds text to the end of the content.
                    using (var formattedText = new PdfFormattedText())
                    {
                        formattedText.Font = new PdfFont(new PdfFontFace("Helvetica"), 16.0);
                        // Set "orange" color
                        formattedText.Color = PdfColor.FromRgb(1, 0.647, 0);
                        formattedText.AppendLine($"Page {pageNum++}");
                        page.Content.DrawText(formattedText, new PdfPoint((page.CropBox.Width / 2) - formattedText.Width, page.CropBox.Height - 50));

                        // Because of the trial version, we'll add page numbers only to two pages.
                        if (pageNum > 2)
                            break;
                    }
                }
                document.Save(outFile);
            }
            // Show the result.
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outFile) { UseShellExecute = true });
        }
    }
}