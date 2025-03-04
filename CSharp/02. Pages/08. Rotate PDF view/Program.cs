using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Sample
{
    class Program
    {
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/rotate-pdf.php
        /// </remarks>
        static void Main()
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            using (var document = new PdfDocument())
            {
                var page = document.Pages.Add();
                // Rotate page content
                page.Rotate = 0;
                // Rotate page view: Landscape or Portrait.
                page.SetMediaBox(page.Size.Height, page.Size.Width);
                var formattedText1 = new PdfFormattedText();
                var text1 = "Hello World";
                formattedText1.FontSize = 15;
                formattedText1.FontFamily = new PdfFontFamily("Calibri");
                formattedText1.Append(text1);
                page.Content.DrawText(formattedText1, new PdfPoint(200, 400));
                document.Save("Output.pdf");
            }
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Output.pdf") { UseShellExecute = true });
        }
    }
}
      
    

    