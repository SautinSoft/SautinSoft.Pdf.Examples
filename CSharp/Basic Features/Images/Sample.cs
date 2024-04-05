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
        /// Details: http://sautinsoft/products/pdf/help/net/developer-guide/export-import-images-to-pdf.php
        /// </remarks>
        static void Main(string[] args)
        {
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
                        break;
                    }
                }
            }
        }
    }
}
