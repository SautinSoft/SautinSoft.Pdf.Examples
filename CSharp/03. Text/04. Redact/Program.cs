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
        /// <summary>
        /// Create a page tree.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/redact.php
        /// </remarks>
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            var document = PdfDocument.Load(pdfFile);
            {
                var texts = document.Pages[0].Content.GetText().Find("the");
                foreach (var tab in texts)
                {
                    var text = new PdfFormattedText();
                    var bounds = tab.Bounds;
                    text.Font = tab.Format.Text.Font;
                    tab.Redact();
                    text.Append("-");
                    document.Pages[0].Content.DrawText(text, new PdfPoint(bounds.Left, bounds.Bottom));
                }

                document.Save("out.pdf");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("out.pdf") { UseShellExecute = true });
        }
    }
}