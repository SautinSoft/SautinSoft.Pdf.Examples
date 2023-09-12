using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        static void Main(string[] args)
        {
            // Create new document.
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
                document.Save("converted-png-image.pdf");
            }
        }
    }
}
