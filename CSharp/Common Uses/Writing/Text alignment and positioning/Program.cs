using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content;

class Program
{
    static void Main()
    {
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();
            double margin = 10;
            using (var formattedText = new PdfFormattedText())
            {
                formattedText.TextAlignment = PdfTextAlignment.Left;
                formattedText.MaxTextWidth = 100;
                formattedText.Append("This text is left aligned, ").
                Append("placed in the top-left corner of the page and ").
                Append("its width should not exceed 100 points.");
                page.Content.DrawText(formattedText,
                    new PdfPoint(margin,
                    page.CropBox.Top - margin - formattedText.Height));
                formattedText.Clear();
                formattedText.TextAlignment = PdfTextAlignment.Center;
                formattedText.MaxTextWidth = 200;
                formattedText.Append("This text is center aligned, ").
                Append("placed in the top-center part of the page ").
                Append("and its width should not exceed 200 points.");
                page.Content.DrawText(formattedText,
                    new PdfPoint((page.CropBox.Width - formattedText.MaxTextWidth) / 2,
                    page.CropBox.Top - margin - formattedText.Height));
                formattedText.Clear();
                formattedText.TextAlignment = PdfTextAlignment.Right;
                formattedText.MaxTextWidth = 100;
                formattedText.Append("This text is right aligned, ").
                Append("placed in the top-right corner of the page ").
                Append("and its width should not exceed 100 points.");
                page.Content.DrawText(formattedText,
                    new PdfPoint(page.CropBox.Width - margin - formattedText.MaxTextWidth,
                    page.CropBox.Top - margin - formattedText.Height));

                formattedText.Clear();

                formattedText.TextAlignment = PdfTextAlignment.Left;
                formattedText.MaxTextWidth = 100;
                formattedText.Append("This text is left aligned, ").
                Append("placed in the bottom-left corner of the page and ").
                Append("its width should not exceed 100 points.");
                page.Content.DrawText(formattedText,
                    new PdfPoint(margin,
                    margin));
                formattedText.Clear();
                formattedText.TextAlignment = PdfTextAlignment.Center;
                formattedText.MaxTextWidth = 200;
                formattedText.Append("This text is center aligned, ").
                Append("placed in the bottom-center part of the page and ").
                Append("its width should not exceed 200 points.");
                page.Content.DrawText(formattedText,
                    new PdfPoint((page.CropBox.Width - formattedText.MaxTextWidth) / 2,
                    margin));
                formattedText.Clear();
                formattedText.TextAlignment = PdfTextAlignment.Right;
                formattedText.MaxTextWidth = 100;
                formattedText.Append("This text is right aligned, ").
                Append("placed in the bottom-right corner of the page and ").
                Append("its width should not exceed 100 points.");
                page.Content.DrawText(formattedText,
                    new PdfPoint(page.CropBox.Width - margin - formattedText.MaxTextWidth,
                    margin));
                formattedText.Clear();
                formattedText.TextAlignment = PdfTextAlignment.Justify;
                formattedText.MaxTextWidths = new double[] { 200, 150, 100 };
                formattedText.Append("This text has justified alignment, ").
                Append("is placed in the center of the page and ").
                Append("its first line should not exceed 200 points, ").
                Append("its second line should not exceed 150 points and ").
                Append("its third and all other lines should not exceed 100 points.");
                // Center the text based on the width of the most lines, which is formattedText.MaxTextWidths[2].
                page.Content.DrawText(formattedText,
                    new PdfPoint((page.CropBox.Width - formattedText.MaxTextWidths[2]) / 2,
                    (page.CropBox.Height - formattedText.Height) / 2));
                document.Save("Alignment and Positioning.pdf");
            }
        }
    }
}