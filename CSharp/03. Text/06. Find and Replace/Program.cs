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
                // Assume we want to find the word "North"
                // and replace it to the "South".
                string textFrom = "North";
                string textTo = "South";

                // Iterate by all pages
                foreach (var page in document.Pages)
                {
                    // Find the text.
                    var texts = page.Content.GetText().Find(textFrom);

                    // Get the text coordinates and font from 1st Element;
                    // Draw the new text.
                    foreach (var text in texts)
                    {
                        foreach (var el in text.Elements)
                        {
                            // Get the text formatting, coordinates;
                            // Draw the new text "South".
                            using (var formattedText = new PdfFormattedText())
                            {
                                formattedText.Language = new PdfLanguage("en-US");

                                double fontSize = Math.Min(text.Bounds.Height, el.Format.Text.Font.Size * el.TextTransform.M11);

                                // Case 1: Use a new font, looks different, but contains all characters.
                                //formattedText.Font = new PdfFont("Helvetica", fontSize);

                                // Case 2: Use the current font, it looks the same, but may contain missing characters.
                                formattedText.Font = el.Format.Text.Font;
                                formattedText.FontSize = fontSize;

                                // Set "Red" color
                                formattedText.Color = PdfColor.FromRgb(1, 0, 0);
                                formattedText.Append(textTo);

                                page.Content.DrawText(formattedText, new PdfPoint(text.Bounds.Left, text.Bounds.Bottom));

                            }
                            break;
                        }
                        // Remove the text.
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