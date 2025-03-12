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
        /// <summary>
        /// Fill in PDF interactive forms.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/flatten-interactive-forms.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

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

                    // Add a new content group to the field's page and
                    // add new form content with the field's appearance form to the content group.
                    // The content group is added so that transformation from the next statement is localized to the content group.
                    var flattenedContent = field.Page.Content.Elements.AddGroup().Elements.AddForm(fieldAppearance);

                    // Translate the form content to the same position on the page that the field is in.
                    var fieldBounds = field.Bounds;
                    flattenedContent.Transform = PdfMatrix.CreateTranslation(fieldBounds.Left, fieldBounds.Bottom);
                }

                // Remove all fields, thus making the document non-interactive,
                // since their appearance is now contained directly in the content of their pages.
                document.Form.Fields.Clear();

                document.Save("FormFlattened.pdf");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("FormFlattened.pdf") { UseShellExecute = true });
        }
    }
}
