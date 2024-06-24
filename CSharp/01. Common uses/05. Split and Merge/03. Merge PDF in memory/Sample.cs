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
        /// Merge PDF documents in memory using C# and .NET.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/merge-pdf-documents-in-memory-using-csharp-and-dotnet.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            MergePdfInMemory();
        }
        static void MergePdfInMemory()
        {
            // In this example we are using files only to get input data and show the result.
            string resultPath = "Result.pdf";
            // The whole merge process will be done completely in memory. 

            // The list with PDFs. The each document stored as bytes array.
            List<byte[]> pdfDocs = new List<byte[]>();
            foreach (var f in Directory.GetFiles(@"..\..\..\", "*.pdf"))
                pdfDocs.Add(File.ReadAllBytes(f));

            // Create a PDF merger.
            var merger = new PdfMerger();

            // Iterate by documents and append them.
            foreach (var pdfDoc in pdfDocs)
                using (var ms = new MemoryStream(pdfDoc))
                    merger.Append(ms);

            // Save the merged PDF to a MemoryStream.
            using (var msMerged = new MemoryStream())
            {
                merger.Save(msMerged);
                // Save the result to a file to show.
                File.WriteAllBytes(resultPath, msMerged.ToArray());
            }

            // Show the result.
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(resultPath) { UseShellExecute = true });
        }
    }
}