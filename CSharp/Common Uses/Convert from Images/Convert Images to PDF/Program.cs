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

        using (var document = new PdfDocument())
        {
            // Add new page.
            var page = document.Pages.Add();

            // Add image from PNG file.
            var image = PdfImage.Load(@"..\..\..\parrot.png");
            page.Content.DrawImage(image, new PdfPoint(0, 0));

            // Set page size.
            page.SetMediaBox(image.Width, image.Height);

            // Save as PDF file.
            document.Save("convert-png-image.pdf");
        }
    }
}