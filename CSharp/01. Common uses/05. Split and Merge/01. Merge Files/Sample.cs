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
        /// Merge PDF files.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/merge-pdf-files.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            MergePdf();
        }

        static void MergePdf()
        {
            string[] inpFiles = new string[] {
                        Path.GetFullPath(@"..\..\..\Simple Text.pdf"),
                        Path.GetFullPath(@"..\..\..\Potato Beetle.pdf"),
                        Path.GetFullPath(@"..\..\..\Text and Graphics.pdf")};

            string outFile = Path.GetFullPath(@"Merged.pdf");

            // Create a new PDF document.
            using (var pdf = new PdfDocument())
            {
                // Merge multiple PDF documents the new single PDF.
                foreach (var inpFile in inpFiles)
                    using (var source = PdfDocument.Load(inpFile))
                        pdf.Pages.Kids.AddClone(source.Pages);

                pdf.Save(outFile);
            }
            // Show the result.
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outFile) { UseShellExecute = true });
        }
    }
}