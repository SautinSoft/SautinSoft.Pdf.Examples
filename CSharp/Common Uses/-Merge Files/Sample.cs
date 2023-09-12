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
            // List of source file names.
            var fileNames = new string[]
            {
            @"..\..\..\MergeFile01.pdf",
            @"..\..\..\MergeFile02.pdf",
            @"..\..\..\MergeFile03.pdf",
            @"..\..\..\MergeFile04.pdf"
            };

            using (var document = new PdfDocument())
            {
                // Merge multiple PDF files into single PDF by loading source documents
                // and cloning all their pages to destination document.
                foreach (var fileName in fileNames)
                    using (var source = PdfDocument.Load(fileName))
                        document.Pages.Kids.AddClone(source.Pages);

                document.Save("Merge Files.pdf");
            }
        }
    }
}
