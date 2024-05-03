using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content;

class Program
{
    static void Main()
    {
    /// <summary>
    /// Add shapes to PDF files.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/draw-images-to-pdf.php
    /// </remarks>
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        using (var document = new PdfDocument())
        {
            // Add a page.
            var page = document.Pages.Add();

            // Load the image from a file.
            var image = PdfImage.Load(@"..\..\..\parrot.png");

            // Set the location of the bottom-left corner of the image.
            // We want the top-left corner of the image to be at location (50, 50)
            // from the top-left corner of the page.
            // NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
            // and the positive y axis extends vertically upward.
            double x = 50, y = page.CropBox.Top - 50 - image.Size.Height;

            // Draw the image to the page.
            page.Content.DrawImage(image, new PdfPoint(x, y));

            document.Save("Parrot.pdf");
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Parrot.pdf") { UseShellExecute = true });
    }
}