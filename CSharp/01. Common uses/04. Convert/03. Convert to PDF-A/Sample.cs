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
        /// Convert PDF to PDF/A using C# and .NET.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-pdf-to-pdfa-using-csharp-and-dotnet.php
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
                // Create PDF save options.
                var pdfOptions = new PdfSaveOptions()
                {
                    // PDF/A-1: Published October 2005 as ISO 19005-1 and based on PDF 1.4.
                    // PDF/A-2: Published June 2011 as ISO 19005-2 and based on PDF 1.7 (ISO 32000-1:2008).
                    // PDF/A-3: Published October 2012 as ISO 19005-3 and based on PDF 1.7 (ISO 32000-1:2008).
                    // PDF/A-4 has been published in 2020 as ISO 19005-4. Based on PDF 2.0.

                    // Select the desired PDF/A version.
                    Version = PdfVersion.PDF_A_4E

                };

                // Save a PDF document.
                document.Save("Convert to PDF-A.pdf", pdfOptions);
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(@"Convert to PDF-A.pdf") { UseShellExecute = true });
        }
    }
}
