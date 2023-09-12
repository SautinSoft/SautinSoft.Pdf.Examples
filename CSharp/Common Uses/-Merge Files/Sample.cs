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
            MergePdf();
        }
        /// <summary>
        /// How to merge PDF documents into a single PDF.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/merge-pdf-files.php
        /// </remarks>
        static void MergePdf()
        {
            string[] inpFiles = new string[] {
                        Path.GetFullPath(@"..\..\..\File1.pdf"),
                        Path.GetFullPath(@"..\..\..\File2.pdf"),
                        Path.GetFullPath(@"..\..\..\File3.pdf")};

            string outFile = Path.GetFullPath(@"Merged.pdf");

            using (var pdf = new PdfDocument())
            {
                // Merge multiple PDF files into single PDF by loading source documents
                // and cloning all their pages to destination document.
                foreach (var inpFile in inpFiles)
                {
                    using (var source = PdfDocument.Load(inpFile))
                    {
                        foreach (var page in source.Pages)
                            pdf.Pages.AddClone(page);
                    }
                }
                pdf.Save(outFile);
            }
            // Show the result PDF document.
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outFile) { UseShellExecute = true });
        }

    }
}
