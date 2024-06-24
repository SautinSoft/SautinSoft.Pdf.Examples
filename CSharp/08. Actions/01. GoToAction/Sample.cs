using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Annotations;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// How to add a go-to action in a PDF document.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/add-go-to-action-in-pdf-document.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            GoToAction();
        }

        static void GoToAction()
        {
            string outFile = "GoToAction.pdf";
            using (var pdf = new PdfDocument())
            {
                // Add a new page.
                var firstPage = pdf.Pages.Add();
                // Add Link annotation.
                var annotation = firstPage.Annotations.AddLink(10, 700, 300, 100);
                annotation.Appearance.BorderColor = PdfColor.FromRgb(0, 255 / 255, 0);

                annotation.Appearance.BorderDashPattern = PdfLineDashPatterns.Solid;
                annotation.Appearance.BorderWidth = 3;
                annotation.Appearance.BorderStyle = PdfBorderStyle.Solid;

                var text = new PdfFormattedText() { FontSize = 16.0 };
                text.Append("Click to go to the second page!");
                firstPage.Content.DrawText(text, new PdfPoint(50, 750));

                // Add a new page.
                var secondPage = pdf.Pages.Add();
                text = new PdfFormattedText() { FontSize = 72.0, Color = PdfColors.Gray };
                text.Append("Page 2");
                secondPage.Content.DrawText(text, new PdfPoint(150, 400));
                // Add an action to go to the second page in the Link annotation.
                var action = annotation.Actions.AddGoToPageView(secondPage, PdfDestinationViewType.FitPage);
                // Save PDF Document.
                pdf.Save(outFile);
            }
            // Show the result.
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outFile) { UseShellExecute = true });
        }
    }
}