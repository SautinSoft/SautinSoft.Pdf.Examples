using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content;

class Program
{
    /// <summary>
    /// Export and import images to PDF file.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/image-positioning-and-transformations.php
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

            // Load the image from a file.
            var image = PdfImage.Load(@"..\..\..\submit.png");

            double margin = 50;

            // Set the location of the first image in the top-left corner of the page (with a specified margin).
            double x = margin;
            double y = page.CropBox.Top - margin - image.Size.Height;

            // Draw the first image.
            page.Content.DrawImage(image, new PdfPoint(x, y));

            // Set the location of the second image in the top-right corner of the page (with the same margin).
            x = page.CropBox.Right - margin - image.Size.Width;
            y = page.CropBox.Top - margin - image.Size.Height;

            // Initialize the transformation.
            var transform = PdfMatrix.Identity;
            // Use the translate operation to position the image.
            transform.Translate(x, y);
            // Use the scale operation to resize the image.
            // NOTE: The unit square of user space, bounded by user coordinates (0, 0) and (1, 1),
            // corresponds to the boundary of the image in the image space.
            transform.Scale(image.Size.Width, image.Size.Height);
            // Use the scale operation to flip the image horizontally.
            transform.Scale(-1, 1, 0.5, 0);

            // Draw the second image.
            page.Content.DrawImage(image, transform);

            // Set the location of the third image in the bottom-left corner of the page (with the same margin).
            x = margin;
            y = margin;

            // Initialize the transformation.
            transform = PdfMatrix.Identity;
            // Use the translate operation to position the image.
            transform.Translate(x, y);
            // Use the scale operation to resize the image.
            transform.Scale(image.Size.Width, image.Size.Height);
            // Use the scale operation to flip the image vertically.
            transform.Scale(1, -1, 0, 0.5);

            // Draw the third image.
            page.Content.DrawImage(image, transform);

            // Set the location of the fourth image in the bottom-right corner of the page (with the same margin).
            x = page.CropBox.Right - margin - image.Size.Width;
            y = margin;

            // Initialize the transformation.
            transform = PdfMatrix.Identity;
            // Use the translate operation to position the image.
            transform.Translate(x, y);
            // Use the scale operation to resize the image.
            transform.Scale(image.Size.Width, image.Size.Height);
            // Use the scale operation to flip the image horizontally and vertically.
            transform.Scale(-1, -1, 0.5, 0.5);

            // Draw the fourth image.
            page.Content.DrawImage(image, transform);

            document.Save("PositioningTransformations.pdf");
        }
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("PositioningTransformations.pdf") { UseShellExecute = true });
    }
}