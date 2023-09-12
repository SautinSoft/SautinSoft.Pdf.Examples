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
            string pdfFile = Path.GetFullPath(@"..\..\..\FormFilled.pdf");

            using (var document = PdfDocument.Load(pdfFile))
                document.Form.ExportData("Form Data.fdf");
        }
    }
}
