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
        /// Find and Replace text in PDF document using C# and .NET
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/find-and-replace-text-in-pdf-document-csharp-dotnet.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string inpFile = Path.GetFullPath(@"..\..\..\simple text.pdf");
            string outFile = Path.GetFullPath("Result.pdf");

            using (PdfDocument document = PdfDocument.Load(inpFile))
            {
                foreach (var page in document.Pages)
                {
                    var search = page.Content.GetText().Find("North");
                    foreach (var text in search)
                    {
                        var element = text.Elements.First();
                        var font = element.Format.Text.Font;
                        double size = Math.Min(text.Bounds.Height, font.Size * element.TextTransform.M11);

                        using (var formattedText = new PdfFormattedText() { Font = font, Color = PdfColor.FromRgb(1, 0, 0), FontSize = size })
                        {
                            formattedText.Append("South");
                            page.Content.DrawText(formattedText, new PdfPoint(text.Bounds.Left, text.Bounds.Bottom));
                        }
                        text.Redact();
                    }
                }
                document.Save(outFile);
            }
            // Show the result.
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outFile) { UseShellExecute = true });
        }
    }
}