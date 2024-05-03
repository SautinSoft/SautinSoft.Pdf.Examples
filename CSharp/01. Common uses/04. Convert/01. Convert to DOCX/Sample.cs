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
        /// <summary>
        /// Convert Images to PDF.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-pdf-to-docx.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            // Load a PDF document.
            using (var document = PdfDocument.Load(Path.GetFullPath(@"..\..\..\simple text.pdf")))
            {  
                // Save a PDF document to a DOCX file.
                document.Save("Output.docx");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Output.docx") { UseShellExecute = true });
        }
    }
}
