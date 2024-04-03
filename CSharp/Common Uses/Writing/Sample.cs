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
            using (var document = new PdfDocument())
            {
                // Add a page.
                var page = document.Pages.Add();

                using (var formattedText = new PdfFormattedText())
                {
                    // Set font family and size.
                    // All text appended next uses the specified font family and size.

                    formattedText.FontFamily = new PdfFontFamily("Arial");
                    formattedText.FontWeight = PdfFontWeight.Black;
                    formattedText.TextFormattingMode = PdfTextFormattingMode.HarfBuzz;
                    formattedText.FontSize = 24;
                    formattedText.TextAlignment = PdfTextAlignment.Left;

                    formattedText.AppendLine("Hello, World!");
                    // Reset font family and size for all text appended next.
                    formattedText.FontFamily = new PdfFontFamily("Calibri");
                    formattedText.FontWeight = PdfFontWeight.Normal;
                    formattedText.FontSize = 12;

                    formattedText.AppendLine(" with ");

                    // Set font style and color for all text appended next.
                    formattedText.FontStyle = PdfFontStyle.Italic;
                    formattedText.Color = PdfColor.FromRgb(1, 0, 0);

                    formattedText.Append("SautinSoft.Pdf.Document");

                    // Reset font style and color for all text appended next.
                    formattedText.FontStyle = PdfFontStyle.Normal;
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);

                    formattedText.Append(" component!");

                    // Set the location of the bottom-left corner of the text.
                    // We want top-left corner of the text to be at location (100, 100)
                    // from the top-left corner of the page.
                    // NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
                    // and the positive y axis extends vertically upward.
                    double x = 100, y = page.CropBox.Top - 100 - formattedText.Height;

                    // Draw text to the page.
                    page.Content.DrawText(formattedText, new PdfPoint(x, y));

                    formattedText.TextAlignment = PdfTextAlignment.Center;
                    x = page.CropBox.Width / 2; y = page.CropBox.Top / 2;

                    page.Content.DrawText(formattedText, new PdfPoint(x, y));

                    formattedText.TextAlignment = PdfTextAlignment.Right;
                    x = page.CropBox.Width - 100; y = 100;

                    page.Content.DrawText(formattedText, new PdfPoint(x, y));
                }

                document.Save("Writing.pdf");
            }

        }
    }
}
