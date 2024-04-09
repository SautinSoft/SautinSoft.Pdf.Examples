using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        static void Main(string[] args)
        {

            // Load a PDF document.
            using (var document = PdfDocument.Load(@"..\..\..\simple text.pdf"))
            {
                var page = document.Pages[0];
                var pageContent = page.Content;
                var texts = pageContent.GetText().Find("Island");

                foreach (var text in texts)
                {
                    using var formattedText = new PdfFormattedText();
                    formattedText.Append("Land");
                    pageContent.DrawText(formattedText, new PdfPoint(text.Bounds.Left, text.Bounds.Bottom));
                    text.Redact();
                }
                document.Save("Output.pdf");
            }
            
    }
    }
}
