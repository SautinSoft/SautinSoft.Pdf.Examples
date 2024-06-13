using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content.Patterns;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Objects;
using SautinSoft.Pdf.Content.Colors;

class Program
{
    /// <summary>
    /// Patterns
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/pdf-content-formatting-patterns.php
    /// </remarks>
    static void Main()
    {
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        using (var document = new PdfDocument())
        {
                // The uncolored tiling pattern should not specify the color of its content, instead the outer element that uses the uncolored tiling pattern will specify the color of the tiling pattern content.
                var uncoloredTilingPattern = new PdfTilingPattern(document, new PdfSize(100, 100)) { IsColored = false };
                // Begin editing the pattern cell.
                uncoloredTilingPattern.Content.BeginEdit();
                // The tiling pattern cell contains two triangles that are filled with color specified by the outer element that uses the uncolored tiling pattern.
                var path = uncoloredTilingPattern.Content.Elements.AddPath();
                path.BeginSubpath(0, 0).LineTo(50, 0).LineTo(50, 100).CloseSubpath();
                path.Format.Fill.IsApplied = true;
                path.BeginSubpath(50, 0).LineTo(100, 0).LineTo(100, 100).CloseSubpath();
                path.Format.Fill.IsApplied = true;
                // End editing the pattern cell.
                uncoloredTilingPattern.Content.EndEdit();

                // Create an uncolored tiling Pattern color space.
                // as specified in http://www.adobe.com/content/dam/acom/en/devnet/pdf/PDF32000_2008.pdf#page=186.
                // The underlying color space is DeviceRGB and colorants will be specified in DeviceRGB.
                var uncoloredTilingPatternColorSpaceArray = PdfArray.Create(2);
                uncoloredTilingPatternColorSpaceArray.Add(PdfName.Create("Pattern"));
                uncoloredTilingPatternColorSpaceArray.Add(PdfName.Create("DeviceRGB"));
                var uncoloredTilingPatternColorSpace = PdfColorSpace.FromArray(uncoloredTilingPatternColorSpaceArray);

                var page = document.Pages.Add();

                // Add a background rectangle over the entire page that shows how the tiling pattern, by default, starts from the bottom-left corner of the page.
                var mediaBox = page.MediaBox;
                var backgroundRect = page.Content.Elements.AddPath();
                backgroundRect.AddRectangle(mediaBox.Left, mediaBox.Bottom, mediaBox.Width, mediaBox.Height);
                var format = backgroundRect.Format;
                format.Fill.IsApplied = true;
                format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 0, 0, 0);
                format.Fill.Opacity = 0.2;

                // Add a rectangle that is filled with the red (red = 1, green = 0, blue = 0) pattern.
                var redRect = page.Content.Elements.AddPath();
                redRect.AddRectangle(75, 575, 200, 100);
                format = redRect.Format;
                format.Fill.IsApplied = true;
                format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 1, 0, 0);
                format.Stroke.IsApplied = true;

                // Add a rectangle that is filled with the same pattern, but this time the pattern's color is green (red = 0, green = 1, blue = 0).
                var greenRect = page.Content.Elements.AddClone(redRect);
                greenRect.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -150));
                greenRect.Format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 0, 1, 0);

                // Add a rectangle that is filled with the same pattern, but this time the pattern's color is blue (red = 0, green = 0, blue = 1).
                var blueRect = page.Content.Elements.AddClone(greenRect);
                blueRect.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -150));
                blueRect.Format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 0, 0, 1);

                // The colored tiling pattern specifies the color of its content.
                var coloredTilingPattern = new PdfTilingPattern(document, new PdfSize(100, 100));
                // Begin editing the pattern cell.
                coloredTilingPattern.Content.BeginEdit();
                // The tiling pattern cell contains two triangles. The first one is filled with the red color and the second one is filled with the green color.
                path = coloredTilingPattern.Content.Elements.AddPath();
                path.BeginSubpath(0, 0).LineTo(50, 0).LineTo(50, 100).CloseSubpath();
                format = path.Format;
                format.Fill.IsApplied = true;
                format.Fill.Color = PdfColors.Red;
                path = coloredTilingPattern.Content.Elements.AddPath();
                path.BeginSubpath(50, 0).LineTo(100, 0).LineTo(100, 100).CloseSubpath();
                format = path.Format;
                format.Fill.IsApplied = true;
                format.Fill.Color = PdfColors.Green;
                // End editing the pattern cell.
                coloredTilingPattern.Content.EndEdit();

                // Add a rectangle that is filled with the colored (red-green) tiling pattern.
                var redGreenRect = page.Content.Elements.AddPath();
                redGreenRect.AddRectangle(325, 275, 200, 400);
                format = redGreenRect.Format;
                format.Fill.IsApplied = true;
                format.Fill.Color = PdfColor.FromPattern(coloredTilingPattern);
                format.Stroke.IsApplied = true;

                document.Save("Patterns.pdf");
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Patterns.pdf") { UseShellExecute = true });
    }
}