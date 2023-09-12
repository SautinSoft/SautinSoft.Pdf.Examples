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
            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");
            
            using (var document = PdfDocument.Load(pdfFile))
            {
                foreach (var page in document.Pages)
                {
                    Console.WriteLine(page.Content.ToString());
                }
            }
        }
    }
}
