using System;
using System.IO;
using System.Reflection.Metadata;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {

        static void Main(string[] args)
        {
            /// <remarks>
            /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/save-pdf.php
            /// </remarks>

            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            // Load a PDF document.
            using (var document = PdfDocument.Load(Path.GetFullPath(@"..\..\..\simple text.pdf")))
            {
                // Save a PDF document.
                document.Save("Output.pdf");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(@"Output.pdf") { UseShellExecute = true });
        }
    }
}