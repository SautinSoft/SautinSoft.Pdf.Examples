using System;
using System.Drawing;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using static System.Net.WebRequestMethods;

namespace Sample
{
    class Sample
    {
        static void Main(string[] args)
        {


            using (var document = new PdfDocument())
            {
                var page = document.Pages.Add();
                double x = 50;
                double y = page.Size.Height;

                using (var formattedText = new PdfFormattedText())
                {
                    formattedText.Append("The following QR code is imported from a PDF that was created with SautinSoft.Document.");
                    page.Content.DrawText(formattedText, new PdfPoint(x, y - 50));
                }

                // Create barcode and save it as PDF stream.
                var barcode = CreateBarcode("1234567890");
                var barcodeAsPdf = new MemoryStream();
                barcode.FormatDrawing().Save(barcodeAsPdf, SautinSoft.Document.SaveOptions.PdfDefault);

                // Add barcode to PDF page.
                using (var barcodeDocument = PdfDocument.Load(barcodeAsPdf))
                    document.AppendPage(barcodeDocument, 0, 0, new PdfPoint(x, y - barcode.Layout.Size.Height - 60));

                document.Save("Barcode.pdf");
            }
        }
        static SautinSoft.Document.TextBox CreateBarcode(string qrCode)
        {
            var document = new DocumentModel();
            document.DefaultParagraphFormat.SpaceAfter = 0;
            document.DefaultParagraphFormat.LineSpacing = 1;

            var textBox = new SautinSoft.Document.TextBox(document, Layout.Inline(0, 0, SautinSoft.Document.LengthUnit.Point),
                new Paragraph(document,
                    new Field(document, FieldType.DisplayBarcode, $"{qrCode} QR")));

            document.Sections.Add(
                new SautinSoft.Document.Section(document,
                    new Paragraph(document, textBox)));

            textBox.TextBoxFormat.InternalMargin = new Padding(0);
            textBox.TextBoxFormat.AutoFit = SautinSoft.Document.TextAutoFit.ResizeShapeToFitText;
            document.GetPaginator(new SautinSoft.Document.PaginatorOptions() { UpdateTextBoxHeights = true });

            double size = textBox.Layout.Size.Height;
            textBox.Layout.Size = new Size(size, size);

            return textBox;
        }
    }
}
