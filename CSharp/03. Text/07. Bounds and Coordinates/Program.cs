using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using System.Linq;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Find a specific text on page #2 in the PDF and show Bounds, Coordinates, Points.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\sample.pdf");

            using var document = PdfDocument.Load(pdfFile);
            // Page #2:
            var page = document.Pages[1];

            var foundText = page.Content.GetText().Find("Best Beaches:").FirstOrDefault();
            if (foundText != null)
                Console.WriteLine(foundText.Bounds);
        }
    }
}