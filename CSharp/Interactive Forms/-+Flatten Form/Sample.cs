using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Forms;

namespace Sample
{
    class Sample
    {
        static void Main(string[] args)
        {
            string pdfFile = Path.GetFullPath(@"..\..\..\FormFilled.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                // A flag specifying whether to construct appearance for all form fields in the document.
                bool needAppearances = document.Form.NeedAppearances;

                foreach (var field in document.Form.Fields)
                {
                    // Do not flatten button fields.
                    if (field.FieldType == PdfFieldType.Button)
                        continue;

                    // Construct appearance, if needed.
                    if (needAppearances)
                        field.Appearance.Refresh();

                    // Get the field's appearance form.
                    var fieldAppearance = field.Appearance.Get();

                    // If the field doesn't have an appearance, skip it.
                    if (fieldAppearance == null)
                        continue;

                    // Draw field's appearance on the page.
                    field.Page.Content.DrawAnnotation(field);
                }

                // Remove all fields, thus making the document non-interactive,
                // since their appearance is now contained directly in the content of their pages.
                document.Form.Fields.Clear();

                document.Save("FormFlattened.pdf");
            }
        }
    }
}
