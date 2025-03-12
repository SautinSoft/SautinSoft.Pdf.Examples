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
        /// Read text from PDF.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/read-text-from-pdf-files.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");
            
            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            // Load PDF Document.
            using (var document = PdfDocument.Load(pdfFile))
            {
                foreach (var page in document.Pages)
                {
                    // Write text from pdf file to console.
                    Console.WriteLine(page.Content.ToString());
                }
            }
        }
    }
}