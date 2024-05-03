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
            /// <remarks>
            /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/open-pdf.php
            /// </remarks>
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                if (document != null)
                {
                    Console.WriteLine("File opened successfully");
                }
            }
        }
    }
}