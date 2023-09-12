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
            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            using (var document = new PdfDocument())
            {
                var page = document.Pages.Add();

                var secondPage = document.Pages.Add();

                var pageWidth = page.Size.Width;

                using (var formattedText = new PdfFormattedText())
                {
                    formattedText.FontSize = 24;
                    formattedText.Append("First page");
                    double y = 700;
                    var origin = new PdfPoint((pageWidth - formattedText.Width) / 2, y);
                    page.Content.DrawText(formattedText, origin);

                    var image = PdfImage.Load("SautinSoft.png");
                    y -= image.Size.Height + 100;
                    origin = new PdfPoint((pageWidth - image.Size.Width) / 2, y);
                    page.Content.DrawImage(image, origin);

                    // Add a link annotation over the drawn image that opens a website.
                    var link = page.Annotations.AddLink(origin.X, origin.Y, image.Size.Width, image.Size.Height);
                    link.Actions.AddOpenWebLink("https://www.gemboxsoftware.com/");

                    formattedText.Clear();
                    formattedText.Append("Open file");
                    y -= formattedText.Height + 100;
                    origin = new PdfPoint((pageWidth - formattedText.Width) / 2, y);
                    page.Content.DrawText(formattedText, origin);

                    // Add a link annotation over the drawn text that opens a file.
                    link = page.Annotations.AddLink(origin.X, origin.Y, formattedText.Width, formattedText.Height);
                    link.Actions.AddOpenFile(pdfFile);

                    formattedText.Clear();
                    formattedText.Append("Go to second page");
                    y -= formattedText.Height + 100;
                    origin = new PdfPoint((pageWidth - formattedText.Width) / 2, y);
                    page.Content.DrawText(formattedText, origin);

                    // Add a link annotation over the drawn text that goes to the second page.
                    link = page.Annotations.AddLink(origin.X, origin.Y, formattedText.Width, formattedText.Height);
                    link.Actions.AddGoToPageView(secondPage, PdfDestinationViewType.FitPage);

                    formattedText.Clear();
                    formattedText.Append("Second page");
                    origin = new PdfPoint((pageWidth - formattedText.Width) / 2, 700);
                    secondPage.Content.DrawText(formattedText, origin);
                }

                document.Save("Hyperlinks.pdf");
            }
        }
    }
}
