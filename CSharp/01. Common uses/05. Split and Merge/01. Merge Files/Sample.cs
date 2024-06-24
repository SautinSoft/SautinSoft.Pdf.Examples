using System;
using System.IO;
using System.Collections.Generic;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Facades;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Merge PDF files.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/merge-pdf-files.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            MergePdfFiles();
        }

        static void MergePdfFiles()
        {
            var inpFiles = new List<string>(Directory.GetFiles(Path.GetFullPath(@"..\..\..\"), "*.pdf"));
            string outFile = Path.GetFullPath(@"Merged.pdf");

            // Create a PDF merger.
            var merger = new PdfMerger();

            // Merge multiple PDF documents to the one.
            foreach (var inpFile in inpFiles)
                merger.Append(inpFile);

            merger.Save(outFile);

            // Show the result.
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outFile) { UseShellExecute = true });
        }
    }
}