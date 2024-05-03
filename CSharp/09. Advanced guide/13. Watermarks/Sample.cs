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
        /// Watermarks.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/watermarks.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                // Load the watermark from a file.
                var image = PdfImage.Load(@"..\..\..\WatermarkImage.png");

                foreach (var page in document.Pages)
                {
                    // Make sure the watermark is correctly transformed even if
                    // the page has a custom crop box origin, is rotated, or has custom units.
                    var transform = page.Transform;
                    transform.Invert();

                    // Center the watermark on the page.
                    var pageSize = page.Size;
                    transform.Translate((pageSize.Width - 1) / 2, (pageSize.Height - 1) / 2);

                    // Calculate the scaling factor so that the watermark fits the page.
                    var cropBox = page.CropBox;
                    var scale = Math.Min(cropBox.Width, cropBox.Height);

                    // Scale the watermark so that it fits the page.
                    transform.Scale(scale, scale, 0.5, 0.5);

                    // Draw the centered and scaled watermark.
                    page.Content.DrawImage(image, transform);
                }

                document.Save("Watermark Images.pdf");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Watermark Images.pdf") { UseShellExecute = true });
        }
    }
}
