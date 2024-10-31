using System;
using System.IO;
using System.Reflection;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {

        /// <summary>
        /// PDF Reordering (Moving pages).
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/page-reordering.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            // Load a PDF document.
            using (var document = PdfDocument.Load(Path.GetFullPath(@"..\..\..\simple text.pdf")))
            {
                // Moving the first page to the third
                var page = document.Pages[0];
                document.Pages.Remove(page);
                document.Pages.InsertClone(2, page);

                // Save a PDF document.
                document.Save("Output.pdf");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(@"Output.pdf") { UseShellExecute = true });
        }
    }
}