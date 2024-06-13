using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Annotations;

class Program
{
    /// <summary>
    /// Text Annotations.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/link-annotations.php
    /// </remarks>
    static void Main()
    {
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        using (var document = new PdfDocument())
        {
            // Add a page.
            var page = document.Pages.Add();

            using (var formattedText = new PdfFormattedText())
            {
                // Set font family and size.
                // All text appended next uses the specified font family and size.
                formattedText.FontFamily = new PdfFontFamily("Calibri");
                formattedText.FontSize = 12;

                formattedText.AppendLine("Hello World");

                // Reset font family and size for all text appended next.
                formattedText.FontFamily = new PdfFontFamily("Times New Roman");
                formattedText.FontSize = 14;
                formattedText.FontStyle = PdfFontStyle.Italic;
                formattedText.Color = PdfColor.FromRgb(1, 0, 0);
                formattedText.AppendLine(" This message was ");

                // Set font style and color for all text appended next.
                formattedText.FontFamily = new PdfFontFamily("Archi");
                formattedText.FontSize = 18;

                formattedText.Append("created by SautinSoft");

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

                //Find text "SautinSoft" and add Link annotations...
                var text = page.Content.GetText().Find("SautinSoft");
                foreach (var item in text)
                {
                    var link = page.Annotations.AddLink(item.Bounds.Left, item.Bounds.Bottom, item.Bounds.Width, item.Bounds.Height);
                    link.Actions.AddOpenWebLink("https://sautinsoft.com/");
                }
            }
            document.Save("Writing.pdf");
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Writing.pdf") { UseShellExecute = true });
    }
}