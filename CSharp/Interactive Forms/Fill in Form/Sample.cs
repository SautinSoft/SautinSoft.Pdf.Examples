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
        /// Fill in PDF interactive forms.
        /// </summary>
        /// <remarks>
        /// Details: http://sautinsoft/products/pdf/help/net/developer-guide/fill-in-pdf-interactive-forms.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\Form.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                document.Form.Fields["FullName"].Value = "Jane Doe";
                document.Form.Fields["ID"].Value = "0123456789";
                document.Form.Fields["Gender"].Value = "Female";
                document.Form.Fields["Married"].Value = "Yes";
                document.Form.Fields["City"].Value = "Berlin";
                document.Form.Fields["Language"].Value = new string[] { "German", "Italian" };
                document.Form.Fields["Notes"].Value = "Notes first line\rNotes second line\rNotes third line";

                document.Save("FormFilled.pdf");
            }

        }
    }
}
