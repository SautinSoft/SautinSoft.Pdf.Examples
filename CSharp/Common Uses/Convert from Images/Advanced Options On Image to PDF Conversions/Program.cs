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

        // Create new document.
        using (var document = new PdfDocument())
        {
            // Load image from PNG file.
            var image = PdfImage.Load(@"..\..\..\JPEG2.jpg");

            double width = image.Width;
            double height = image.Height;
            double ratio = width / height;

            // Add image four times, each time with 20% smaller size.
            for (int i = 0; i < 4; i++)
            {
                width *= 0.8;
                height = width / ratio;

                var page = document.Pages.Add();
                page.Content.DrawImage(image, new PdfPoint(0, 0), new PdfSize(width, height));
                page.SetMediaBox(width, height);
            }

            document.Save("convert-scaled-images.pdf");
        }
    }
}