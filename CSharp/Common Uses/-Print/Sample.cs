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
            using (PdfDocument document = PdfDocument.Load(@"..\..\..\simple text.pdf"))
            {
                // Print Word document to default printer.
                string printer = null;
                document.Print(printer);
            }

        }
    }
}
