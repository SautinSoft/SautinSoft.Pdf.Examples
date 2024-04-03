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
       
        static void MergePdf()
        {
            string[] inpFiles = new string[] {
                        Path.GetFullPath(@"d:\Work\SDK\Products\PDF .Net\GitHub\Code samples\CSharp\Common Uses\-Merge Files\MergeFile01.pdf" ),
                        Path.GetFullPath(@"d:\Work\SDK\Products\PDF .Net\GitHub\Code samples\CSharp\Common Uses\-Merge Files\MergeFile02.pdf" ),
                        Path.GetFullPath(@"d:\Work\SDK\Products\PDF .Net\GitHub\Code samples\CSharp\Common Uses\-Merge Files\MergeFile03.pdf" )};

            string outFile = Path.GetFullPath(@"Merged.pdf");

            using (var pdf = new PdfDocument())
            {
                // Merge multiple PDF files into single PDF.
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