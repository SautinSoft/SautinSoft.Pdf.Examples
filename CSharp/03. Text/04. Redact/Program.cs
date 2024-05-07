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
        /// Create a page tree.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/redact.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            var document = PdfDocument.Load(pdfFile);
            {
                // Find all occurrences of a given text in a pdf file.
                var texts = document.Pages[0].Content.GetText().Find("the");
                foreach (var tab in texts)
                {
                    var text = new PdfFormattedText();
                    var bounds = tab.Bounds;
                    text.Font = tab.Format.Text.Font;
                    // Remove text.
                    tab.Redact();
                    text.Append("-");
                    // Add new text in this bounds.
                    document.Pages[0].Content.DrawText(text, new PdfPoint(bounds.Left, bounds.Bottom));
                }
                // Save PDF Document.
                document.Save("out.pdf");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("out.pdf") { UseShellExecute = true });
        }
    }
}