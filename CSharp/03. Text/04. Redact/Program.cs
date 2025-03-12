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
        /// Redact
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/redact.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            var document = PdfDocument.Load(pdfFile);
            {
                // Assume we want to redact the word "North".
                string textToRedact = "North";

                var page = document.Pages[0];
                var texts = page.Content.GetText().Find(textToRedact);
                
                foreach (var text in texts)
                {
                    text.Redact();
                    // If you want, draw a green rectangle 
                    // at the places where was the text.
                    var bounds = text.Bounds;
                    var rectangle = page.Content.Elements.AddPath().AddRectangle(new PdfPoint(bounds.Left, bounds.Bottom), new PdfSize(bounds.Width, bounds.Height));
                    rectangle.Format.Fill.IsApplied = true;
                    rectangle.Format.Fill.Color = PdfColor.FromRgb(0, 1, 0);
                }
                // Save PDF Document.
                document.Save("out.pdf");
            }
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("out.pdf") { UseShellExecute = true });
        }
    }
}