using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content;

class Program
{
    /// <summary>
    /// Filling
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/pdf-content-formatting-filling.php
    /// </remarks>
    static void Main()
    {
        // Before starting this example, please get a free 100-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            // PdfFormattedText currently supports just Device color spaces (DeviceGray, DeviceRGB, and DeviceCMYK).
            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Font = new PdfFont("Helvetica", 100);

                // Make the text fill black (in DeviceGray color space) and 50% opaque.
                formattedText.Color = PdfColor.FromGray(0);
                // In PDF, opacity is defined separately from the color.
                formattedText.Opacity = 0.5;
                formattedText.Append("Hello world!");

                page.Content.DrawText(formattedText, new PdfPoint(50, 700));
            }

            // Path filled with non-zero winding number rule.
            var path = page.Content.Elements.AddPath();
            var center = new PdfPoint(300, 500);
            double radius = 150, cos1 = Math.Cos(Math.PI / 10), sin1 = Math.Sin(Math.PI / 10), cos2 = Math.Cos(Math.PI / 5), sin2 = Math.Sin(Math.PI / 5);
            // Create a five-point star.
            path.
            BeginSubpath(center.X - sin2 * radius, center.Y - cos2 * radius). // Start from the point in the bottom-left corner.
            LineTo(center.X + cos1 * radius, center.Y + sin1 * radius). // Continue to the point in the upper-right corner.
            LineTo(center.X - cos1 * radius, center.Y + sin1 * radius). // Continue to the point in the upper-left corner.
            LineTo(center.X + sin2 * radius, center.Y - cos2 * radius). // Continue to the point in the bottom-right corner.
            LineTo(center.X, center.Y + radius). // Continue to the point in the upper-center.
            CloseSubpath(); // End with the starting point.
            var format = path.Format;
            format.Fill.IsApplied = true;
            format.Fill.Rule = PdfFillRule.NonzeroWindingNumber;
            // Make the path fill red (in DeviceRGB color space) and 40% opaque.
            format.Fill.Color = PdfColor.FromRgb(1, 0, 0);
            format.Fill.Opacity = 0.4;

            // Path filled with even-odd rule.
            path = page.Content.Elements.AddClone(path);
            path.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -300));
            format = path.Format;
            format.Fill.IsApplied = true;
            format.Fill.Rule = PdfFillRule.EvenOdd;
            // Make the path fill yellow (in DeviceCMYK color space) and 60% opaque.
            format.Fill.Color = PdfColor.FromCmyk(0, 0, 1, 0);
            format.Fill.Opacity = 0.6;

            document.Save("Filling.pdf");
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Filling.pdf") { UseShellExecute = true });
    }
}