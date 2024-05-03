using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content;

class Program
{
    static void Main()
    {
            /// <summary>
            /// How to add a go-to action in a PDF document.
            /// </summary>
            /// <remarks>
            /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/pdf-content-formatting-clipping.php
            /// </remarks>
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            // Add a new content group. Clipping is localized to the content group.
            var textGroup = page.Content.Elements.AddGroup();
            // Draw text in the content group.
            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Font = new PdfFont("Helvetica", 96);

                formattedText.Append("Hello world!");

                textGroup.DrawText(formattedText, new PdfPoint(50, 700));
            }
            // Stroke all text elements in the group (to visualize their bounds) and set them as a clipping path.
            var format = textGroup.Format;
            format.Fill.IsApplied = false;
            format.Stroke.IsApplied = true;
            format.Clip.IsApplied = true;
            // Draw an image in the same content group as the text.
            // The image will be clipped to the text.
            var image = PdfImage.Load(@"..\..\..\JPEG2.jpg");
            textGroup.DrawImage(image, new PdfPoint(50, 700), new PdfSize(500, 100));

            // Add a new content group. Clipping is localized to the content group.
            var pathGroup = page.Content.Elements.AddGroup();
            // Add a diamond-like path to the content group.
            pathGroup.Elements.AddPath().BeginSubpath(50, 550).LineTo(300, 500).LineTo(550, 550).LineTo(300, 600).CloseSubpath();
            // Stroke all path elements in the group (to visualize their bounds) and set them as a clipping path.
            format = pathGroup.Format;
            format.Fill.IsApplied = false;
            format.Stroke.IsApplied = true;
            format.Clip.IsApplied = true;
            // Draw an image in the same content group as the diamond-like path.
            // The image will be clipped to the diamond-like path.
            pathGroup.DrawImage(image, new PdfPoint(50, 500), new PdfSize(500, 100));

            // Add a new content group. Clipping is localized to the content group.
            pathGroup = page.Content.Elements.AddGroup();
            // Add a star-like path to the content group.
            var path = pathGroup.Elements.AddPath();
            var center = new PdfPoint(150, 300);
            double radius = 100, cos1 = Math.Cos(Math.PI / 10), sin1 = Math.Sin(Math.PI / 10), cos2 = Math.Cos(Math.PI / 5), sin2 = Math.Sin(Math.PI / 5);
            // Create a five-point star.
            path.

            BeginSubpath(center.X - sin2 * radius, center.Y - cos2 * radius). // Start from the point in the bottom-left corner.
            LineTo(center.X + cos1 * radius, center.Y + sin1 * radius). // Continue to the point in the upper-right corner.
            LineTo(center.X - cos1 * radius, center.Y + sin1 * radius). // Continue to the point in the upper-left corner.
            LineTo(center.X + sin2 * radius, center.Y - cos2 * radius). // Continue to the point in the bottom-right corner.
            LineTo(center.X, center.Y + radius). // Continue to the point in the upper-center.
            CloseSubpath(); // End with the starting point.
                            // Stroke a path (to visualize its bounds) and set it as a clipping path using non-zero winding number rule.
            format = path.Format;
            format.Fill.IsApplied = false;
            format.Stroke.IsApplied = true;
            format.Clip.IsApplied = true;
            format.Clip.Rule = PdfFillRule.NonzeroWindingNumber;
            // Draw an image in the same content group as the star-like path.
            // The image will be clipped to the star-like path using non-zero winding number rule.
            pathGroup.DrawImage(image, new PdfPoint(50, 200), new PdfSize(200, 200));

            // Add a new content group. Clipping is localized to the content group.
            pathGroup = page.Content.Elements.AddGroup();
            // Clone a star-like path to the content group and move it down.
            path = pathGroup.Elements.AddClone(path);
            path.Subpaths.Transform(PdfMatrix.CreateTranslation(250, 0));
            // Set the clipping rule to even-odd.
            path.Format.Clip.Rule = PdfFillRule.EvenOdd;
            // Draw an image in the same content group as the star-like path.
            // The image will be clipped to the star-like path using the even-odd rule.
            pathGroup.DrawImage(image, new PdfPoint(300, 200), new PdfSize(200, 200));

            document.Save("Clipping.pdf");
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Clipping.pdf") { UseShellExecute = true });
    }
}