using System;
using System.Globalization;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Header and Footer.
        /// </summary>
        /// <remarks>
        /// Details: http://sautinsoft/products/pdf/help/net/developer-guide/header-footer.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                double marginLeft = 20, marginTop = 10, marginRight = 20, marginBottom = 10;

                using (var formattedText = new PdfFormattedText())
                {
                    formattedText.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));

                    // Add a header with the current date and time to all pages.
                    foreach (var page in document.Pages)
                    {
                        // Set the location of the bottom-left corner of the text.
                        // We want the top-left corner of the text to be at location (marginLeft, marginTop)
                        // from the top-left corner of the page.
                        // NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
                        // and the positive y axis extends vertically upward.
                        double x = marginLeft, y = page.CropBox.Top - marginTop - formattedText.Height;

                        page.Content.DrawText(formattedText, new PdfPoint(x, y));
                    }

                    // Add a footer with the current page number to all pages.
                    int pageCount = document.Pages.Count, pageNumber = 0;
                    foreach (var page in document.Pages)
                    {
                        ++pageNumber;

                        formattedText.Clear();
                        formattedText.Append(string.Format("Page {0} of {1}", pageNumber, pageCount));

                        // Set the location of the bottom-left corner of the text.
                        double x = page.CropBox.Width - marginRight - formattedText.Width, y = marginBottom;

                        page.Content.DrawText(formattedText, new PdfPoint(x, y));
                    }
                }

                document.Save("Header and Footer.pdf");
            }
        }
    }
}
