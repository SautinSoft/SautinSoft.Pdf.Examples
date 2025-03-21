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
        /// Form Xobjects.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/form-xobjects.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            using (var document = new PdfDocument())
            {
                var form = new PdfForm(document, new PdfSize(200, 200));

                form.Content.BeginEdit();

                var textGroup = form.Content.Elements.AddGroup();

                // Add text with the default fill (fill will be inherited from outer PdfFormContent).
                using (var formattedText = new PdfFormattedText())
                {
                    formattedText.Font = new PdfFont("Helvetica", 24);
                    formattedText.Append("Hello world!");

                    // Draw the formatted text at location (50, 150) from the bottom-left corner of the group/form.
                    textGroup.DrawText(formattedText, new PdfPoint(50, 150));
                }

                // Add the same text with a black fill 50 points below the first text.
                var blackText = (PdfTextContent)textGroup.Elements.AddClone(textGroup.Elements.First);
                blackText.TextTransform = PdfMatrix.CreateTranslation(0, -50) * blackText.TextTransform;
                blackText.Format.Fill.Color = PdfColors.Black;

                var pathGroup = form.Content.Elements.AddGroup();

                // Add a rectangle path with the default fill (fill will be inherited from the outer PdfFormContent).
                var path = pathGroup.Elements.AddPath();
                path.AddRectangle(0, 50, 200, 40);
                path.Format.Fill.IsApplied = true;

                // Add the same path with a black fill 50 points below the first path.
                var blackPath = pathGroup.Elements.AddClone(path);
                blackPath.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -50));
                blackPath.Format.Fill.Color = PdfColors.Black;

                form.Content.EndEdit();

                var page = document.Pages.Add();

                // Add the outer PdfFormContent with the default (black) fill.
                // Elements in the inner PdfForm that do not have a fill set, will have the default (black) fill.
                var contentGroup = page.Content.Elements.AddGroup();
                var formContent1 = contentGroup.Elements.AddForm(form);
                formContent1.Transform = PdfMatrix.CreateTranslation(100, 600);

                // Add the outer PdfFormContent with a green fill.
                // Elements in the inner PdfForm that do not have a fill set, will have a green fill.
                contentGroup = page.Content.Elements.AddGroup();
                var formContent2 = contentGroup.Elements.AddForm(form);
                formContent2.Transform = PdfMatrix.CreateTranslation(100, 350);
                formContent2.Format.Fill.Color = PdfColors.Green;

                // Add the outer PdfFormContent with a red fill.
                // Elements in the inner PdfForm that do not have a fill set, will have a red fill.
                contentGroup = page.Content.Elements.AddGroup();
                var formContent3 = contentGroup.Elements.AddClone(formContent1);
                formContent3.Transform = PdfMatrix.CreateTranslation(100, 100);
                formContent3.Format.Fill.Color = PdfColors.Red;

                document.Save("FormXObjects.pdf");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("FormXObjects.pdf") { UseShellExecute = true });
        }
    }
}
