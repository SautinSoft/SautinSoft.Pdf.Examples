using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using System.Linq;
using SautinSoft.Document;

namespace Sample
{
    class Sample
    {
        static void Main(string[] args)
        {
            SplitPdfByPages();
        }
        /// <summary>
        /// How to split PDF document by pages to separate files.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/split-pdf-files.php
        /// </remarks>
        static void SplitPdfByPages()
        {
            string inpFile = Path.GetFullPath(@"..\..\..\simple text.pdf");
            string outDir = Directory.GetCurrentDirectory() + @"\Pages";
            Directory.CreateDirectory(outDir);
            int maxPages = 3;

            using (var pdfDoc = PdfDocument.Load(inpFile))
            {
                for (int page = 0; page < maxPages && page < pdfDoc.Pages.Count; page++)
                {
                    string outFile = Path.Combine(outDir, $"Page {page + 1}.pdf");
                    using (var onePagePdf = new PdfDocument())
                    {
                        onePagePdf.Pages.AddClone(pdfDoc.Pages[page]);
                        onePagePdf.Save(outFile);
                    }
                }
            }
            // Open the directory with one-page PDF files.            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outDir) { UseShellExecute = true });
        }
    }
}
