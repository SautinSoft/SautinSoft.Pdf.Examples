using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using System.Linq;
using SautinSoft.Pdf.Facades;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Split PDF files in C# and .NET.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/split-pdf-files.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            // Split PDF document by pages.
            // The each page will be saves as a separate PDF file: "Page 0.pdf", "Page 1.pdf" ...
            // Can work with relative and absolute paths.
            PdfSplitter.Split(@"..\..\..\005.pdf", PdfLoadOptions.Default, 0,
                int.MaxValue, (pageInd) => $"Page {pageInd}.pdf");

            // The last parameter is "Func" to generate the output file name.
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(@"Page 0.pdf") { UseShellExecute = true });
        }
    }
}
