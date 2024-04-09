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
        /// Clone or import pages between PDF documents.
        /// </summary>
        /// <remarks>
        /// Details: http://sautinsoft/products/pdf/help/net/developer-guide/clone-or-import-pages.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string invoice = Path.GetFullPath(@"..\..\..\invoice.pdf"); 
            string pdffile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            using (var document = PdfDocument.Load(invoice))
            {
                int pageCount = 5;

                // Load a source document.
                using (var source = PdfDocument.Load(pdffile))
                {
                    // Get the number of pages to clone.
                    int cloneCount = Math.Min(pageCount, source.Pages.Count);

                    // Clone the requested number of pages from the source document
                    // and add them to the destination document.
                    for (int i = 0; i < cloneCount; i++)
                        document.Pages.AddClone(source.Pages[i]);
                }

                document.Save("Cloning.pdf");
            }
        }
    }
}
