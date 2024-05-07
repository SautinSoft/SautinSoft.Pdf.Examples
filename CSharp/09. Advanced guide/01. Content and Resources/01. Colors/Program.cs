using System;
using SautinSoft.Pdf;
using System.IO;
using System.Security.Cryptography;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Objects;
using SautinSoft.Pdf.Content.Colors;
using SautinSoft.Pdf.Text;

class Program
{
    /// <summary>
    /// How to add a go-to action in a PDF document.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/pdf-content-formatting-color.php
    /// </remarks>
    static void Main()
    {
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            // PdfFormattedText currently supports just Device color spaces (DeviceGray, DeviceRGB, and DeviceCMYK).
            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Font = new PdfFont("Helvetica", 24);

                // Three different ways to specify gray color in the DeviceGray color space:
                formattedText.Color = PdfColors.Gray;
                formattedText.Append("Hello world! ");
                formattedText.Color = PdfColor.FromGray(0.5);
                formattedText.Append("Hello world! ");
                formattedText.Color = new PdfColor(PdfColorSpace.DeviceGray, 0.5);
                formattedText.AppendLine("Hello world!");

                // Three different ways to specify red color in the DeviceRGB color space:
                formattedText.Color = PdfColors.Red;
                formattedText.Append("Hello world! ");
                formattedText.Color = PdfColor.FromRgb(1, 0, 0);
                formattedText.Append("Hello world! ");
                formattedText.Color = new PdfColor(PdfColorSpace.DeviceRGB, 1, 0, 0);
                formattedText.AppendLine("Hello world!");

                // Three different ways to specify yellow color in the DeviceCMYK color space:
                formattedText.Color = PdfColors.Yellow;
                formattedText.Append("Hello world! ");
                formattedText.Color = PdfColor.FromCmyk(0, 0, 1, 0);
                formattedText.Append("Hello world! ");
                formattedText.Color = new PdfColor(PdfColorSpace.DeviceCMYK, 0, 0, 1, 0);
                formattedText.Append("Hello world!");

                page.Content.DrawText(formattedText, new PdfPoint(100, 500));
            }

            // Create an Indexed color space
            // as specified in http://www.adobe.com/content/dam/acom/en/devnet/pdf/PDF32000_2008.pdf#page=164
            // Base color space is DeviceRGB and the created Indexed color space consists of two colors:
            // at index 0: green color (0x00FF00)
            // at index 1: blue color (0x0000FF)
            var indexedColorSpaceArray = PdfArray.Create(4);
            indexedColorSpaceArray.Add(PdfName.Create("Indexed"));
            indexedColorSpaceArray.Add(PdfName.Create("DeviceRGB"));
            indexedColorSpaceArray.Add(PdfInteger.Create(1));
            indexedColorSpaceArray.Add(PdfString.Create("\x00\xFF\x00\x00\x00\xFF", PdfEncoding.Byte, PdfStringForm.Hexadecimal));
            var indexedColorSpace = PdfColorSpace.FromArray(indexedColorSpaceArray);

            // Add a rectangle.
            // Fill it with color at index 0 (green) of the Indexed color space.
            // Stroke it with color at index 1 (blue) of the Indexed color space.
            var path = page.Content.Elements.AddPath();
            path.AddRectangle(100, 300, 200, 100);
            var format = path.Format;
            format.Fill.IsApplied = true;
            format.Fill.Color = new PdfColor(indexedColorSpace, 0);
            format.Stroke.IsApplied = true;
            format.Stroke.Color = new PdfColor(indexedColorSpace, 1);
            format.Stroke.Width = 5;

            document.Save("Colors.pdf");
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Colors.pdf") { UseShellExecute = true });
    }
}