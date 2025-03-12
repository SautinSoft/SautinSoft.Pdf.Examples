using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Find Tables
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/find-tables.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\tables.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                // Find Tables.
                var tables = document.Pages[0].Content.FindTables();
                Console.WriteLine(tables.Count() + " tables found!");
            }
        }
    }
}