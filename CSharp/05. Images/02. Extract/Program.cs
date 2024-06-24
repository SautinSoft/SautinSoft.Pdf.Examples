using System;
using System.IO;
using System.Linq;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Export and import images to PDF file.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/extract-images-from-pdf.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                // Iterate through PDF pages.
                foreach (var page in document.Pages)
                {
                    // Get all image content elements on the page.
                    var imageElements = page.Content.Elements.All().OfType<PdfImageContent>().ToList();

                    // Export the first image element to an image file.
                    if (imageElements.Count > 0)
                    {
                        imageElements[0].Save("Export Images.jpeg");
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Export Images.jpeg") { UseShellExecute = true });
                        break;
                    }
                }
            }
        }
    }
}