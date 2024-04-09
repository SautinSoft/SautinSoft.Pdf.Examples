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

        string[] jpgs = { @"..\..\..\parrot.png", @"..\..\..\JPEG2.jpg", @"..\..\..\PNG2.png" };
        // Create new document.
        using (var document = new PdfDocument())
        {
            // For each image add new page with margins.
            foreach (var jpg in jpgs)
            {
                var page = document.Pages.Add();
                double margins = 20;

                // Load image from JPG file.
                var image = PdfImage.Load(jpg);

                // Set page size.
                page.SetMediaBox(image.Width + 2 * margins, image.Height + 2 * margins);

                // Draw backgroud color.
                var backgroud = page.Content.Elements.AddPath();
                backgroud.AddRectangle(new PdfPoint(0, 0), page.Size);
                backgroud.Format.Fill.IsApplied = true;
                backgroud.Format.Fill.Color = PdfColor.FromRgb(1, 0, 1);

                // Draw image.
                page.Content.DrawImage(image, new PdfPoint(margins, margins));
            }
            // Save as PDF file.
            document.Save("converted-jpg-images.pdf");
        }
    }
}