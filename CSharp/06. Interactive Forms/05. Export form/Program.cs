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
        /// Export form fields data to fdf/xfdf/json document.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/export-interactive-forms.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\FormFilled.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                //Export form data as fdf stream.
                var fdfFile = new FileStream("fdfOut.fdf", FileMode.Create);
                document.Form.ExportData(fdfFile, SautinSoft.Pdf.Forms.PdfFormDataFormat.FDF);

                //Export form data to xfdf file.
                document.Form.ExportData("xfdfOut.xfdf");

                //Export form data to json file.
                document.Form.ExportData("jsonOut.json");
            }
        }
    }
}