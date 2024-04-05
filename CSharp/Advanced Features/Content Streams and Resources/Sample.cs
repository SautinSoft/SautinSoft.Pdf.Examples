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
        /// <summary>
        /// Manipulate PDF content streams and resources.
        /// </summary>
        /// <remarks>
        /// Details: http://sautinsoft/products/pdf/help/net/developer-guide/content-streams-resources.php
        /// </remarks>
            using (var document = new PdfDocument())
            {
                using (var formattedText = new PdfFormattedText())
                {
                    // Set the font to TrueType font that will be subset and embedded in the document.
                    formattedText.Font = new PdfFont("Calibri", 96);

                    // Draw a single letter on each page.
                    for (int i = 0; i < 2; ++i)
                    {
                        formattedText.Append(((char)('A' + i)).ToString());

                        var page = document.Pages.Add();

                        // Begin editing the page content, but don't end it until all pages are edited.
                        page.Content.BeginEdit();

                        page.Content.DrawText(formattedText, new PdfPoint(100, 500));

                        formattedText.Clear();
                    }
                }

                // End editing of all pages.
                // This will convert the content of each page back to the underlying content stream and the accompanying resource dictionary.
                // Subset of the 'Calibri' font, that contains only glyphs for characters 'A' to 'B' will be calculated just once before being
                // embedded in the document.
                foreach (var page in document.Pages)
                    page.Content.EndEdit();

                document.Save("Content Streams And Resources.pdf");
            }

        }
    }
}
