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
            /// Merge PDF files.
            /// </summary>
            /// <remarks>
            /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/find-tables.php
            /// </remarks>
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\tables.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                var tables = document.Pages[0].Content.FindTables();
                Console.WriteLine(tables.Count() + " tables found!");
            }
        }
    }
}