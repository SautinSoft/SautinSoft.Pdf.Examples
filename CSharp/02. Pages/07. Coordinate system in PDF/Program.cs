using System;
using System.Drawing;
using System.IO;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Coordinate system in PDF document using C# and .NET
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/coordinate-system-in-pdf-document-using-csharp-and-dotnet.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            using (var document = new PdfDocument())
            {
                // Add a page.
                var page = document.Pages.Add();

                // NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
                // and the positive y axis extends vertically upward.
                var pageBounds = page.CropBox;

                // Add a thick red line at the top of the page.
                var line1 = page.Content.Elements.AddPath();
                line1.BeginSubpath(new PdfPoint(596, pageBounds.Top - 0)).
                    LineTo(new PdfPoint(pageBounds.Left - 0, pageBounds.Top - 0));
                var line1Format = line1.Format;
                line1Format.Stroke.IsApplied = true;
                line1Format.Stroke.Width = 5;
                line1Format.Stroke.Color = PdfColor.FromRgb(1, 0, 0);
                // Add a thick blue line to the left of the page.
                var line2 = page.Content.Elements.AddPath();
                line2.BeginSubpath(new PdfPoint(596, pageBounds.Left - 0)).
                    LineTo(new PdfPoint(pageBounds.Right - 0, pageBounds.Top - 0));
                var line2Format = line2.Format;
                line2Format.Stroke.IsApplied = true;
                line2Format.Stroke.Width = 5;
                line2Format.Stroke.Color = PdfColor.FromRgb(0, 0, 1);
                // Add a thick red line at the bottom of the page.
                var line3 = page.Content.Elements.AddPath();
                line3.BeginSubpath(new PdfPoint(596, pageBounds.Left - 0)).
                    LineTo(new PdfPoint(pageBounds.Left - 0, pageBounds.Bottom - 0));
                var line3Format = line3.Format;
                line3Format.Stroke.IsApplied = true;
                line3Format.Stroke.Width = 5;
                line3Format.Stroke.Color = PdfColor.FromRgb(1, 0, 0);
                // Add a thick red line to the right of the page.
                var line4 = page.Content.Elements.AddPath();
                line4.BeginSubpath(new PdfPoint(0, pageBounds.Right - 596)).
                    LineTo(new PdfPoint(pageBounds.Right - 596, pageBounds.Top - 0));
                var line4Format = line4.Format;
                line4Format.Stroke.IsApplied = true;
                line4Format.Stroke.Width = 5;
                line4Format.Stroke.Color = PdfColor.FromRgb(0, 0, 1);

                double margin = 15;
                using (var formattedText = new PdfFormattedText())
                {
                    // Set up and fill the PdfFormattedText object.
                    formattedText.TextAlignment = PdfTextAlignment.Left;
                    formattedText.MaxTextWidth = 100;
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);
                    formattedText.Append("(");
                    formattedText.Color = PdfColor.FromRgb(1, 0, 0);
                    formattedText.Append("0");
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);
                    formattedText.Append(";");
                    formattedText.Color = PdfColor.FromRgb(0, 0, 1);
                    formattedText.Append("842");
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);
                    formattedText.Append(")");
                    // Draw text in the top-left corner of the page.
                    page.Content.DrawText(formattedText,
                        new PdfPoint(margin,
                        page.CropBox.Top - margin - formattedText.Height));

                    // Clear the PdfFormattedText object.
                    formattedText.Clear();

                    // Set up and fill the PdfFormattedText object.
                    formattedText.TextAlignment = PdfTextAlignment.Right;
                    formattedText.MaxTextWidth = 100;
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);
                    formattedText.Append("(");
                    formattedText.Color = PdfColor.FromRgb(1, 0, 0);
                    formattedText.Append("596");
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);
                    formattedText.Append(";");
                    formattedText.Color = PdfColor.FromRgb(0, 0, 1);
                    formattedText.Append("842");
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);
                    formattedText.Append(")");
                    // Draw text in the top-right corner of the page.
                    page.Content.DrawText(formattedText,
                        new PdfPoint(page.CropBox.Width - margin - formattedText.MaxTextWidth,
                    page.CropBox.Top - margin - formattedText.Height));

                    // Clear the PdfFormattedText object.
                    formattedText.Clear();

                    // Set up and fill the PdfFormattedText object.
                    formattedText.TextAlignment = PdfTextAlignment.Right;
                    formattedText.MaxTextWidth = 100;
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);
                    formattedText.Append("(");
                    formattedText.Color = PdfColor.FromRgb(1, 0, 0);
                    formattedText.Append("596");
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);
                    formattedText.Append(";");
                    formattedText.Color = PdfColor.FromRgb(0, 0, 1);
                    formattedText.Append("0");
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);
                    formattedText.Append(")");
                    // Draw text in the bottom-right corner of the page.
                    page.Content.DrawText(formattedText,
                        new PdfPoint(page.CropBox.Width - margin - formattedText.MaxTextWidth,
                    margin));

                    // Clear the PdfFormattedText object.
                    formattedText.Clear();

                    // Set up and fill the PdfFormattedText object.
                    formattedText.TextAlignment = PdfTextAlignment.Left;
                    formattedText.MaxTextWidth = 100;
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);
                    formattedText.Append("(");
                    formattedText.Color = PdfColor.FromRgb(1, 0, 0);
                    formattedText.Append("0");
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);
                    formattedText.Append(";");
                    formattedText.Color = PdfColor.FromRgb(0, 0, 1);
                    formattedText.Append("0");
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0);
                    formattedText.Append(")");
                    // Draw text in the bottom-left corner of the page.
                    page.Content.DrawText(formattedText,
                        new PdfPoint(margin,
                    margin));

                    // Save a PDF Document.
                    document.Save("Coordinate system.pdf");
                }
            }
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Coordinate system.pdf") { UseShellExecute = true });
        }
    }

}

