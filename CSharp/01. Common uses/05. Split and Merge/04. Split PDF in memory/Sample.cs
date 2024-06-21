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
        /// Split PDF documents in memory using C# and .NET.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/merge-pdf-documents-in-memory-using-csharp-and-dotnet.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            SplitPdfInMemory();
        }
        static void SplitPdfInMemory()
        {
            int page = 0;
            using var fs = new FileStream(@"..\..\..\005.pdf", FileMode.Open, FileAccess.ReadWrite);
            foreach (var stream in PdfSplitter.Split(fs, PdfLoadOptions.Default, 0, int.MaxValue))
            {
                using var output = new FileStream($"Page {++page}.pdf", FileMode.Create, FileAccess.ReadWrite);
                stream.CopyTo(output);
            }

            // Show the "Page 5.pdf"
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Page 5.pdf") { UseShellExecute = true });
        }
    }
}