using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Content.Marked;
using SautinSoft.Pdf.Objects;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Create PDF marked content.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/marked-content.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            using (var document = new PdfDocument())
            {
                var page = document.Pages.Add();

                // Surround the path with the marked content start and marked content end elements.
                var markStart = page.Content.Elements.AddMarkStart(new PdfContentMarkTag(PdfContentMarkTagRole.Span));

                var markedProperties = markStart.GetEditableProperties().GetDictionary();

                // Add the path that is a visual representation of the letter 'H'.
                var path = page.Content.Elements.AddPath()
                    .BeginSubpath(100, 600).LineTo(100, 800)
                    .BeginSubpath(100, 700).LineTo(200, 700)
                    .BeginSubpath(200, 600).LineTo(200, 800);

                var format = path.Format;
                format.Stroke.IsApplied = true;
                format.Stroke.Width = 10;

                page.Content.Elements.AddMarkEnd();

                document.Save("MarkedContent.pdf");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("MarkedContent.pdf") { UseShellExecute = true });
        }
    }
}
