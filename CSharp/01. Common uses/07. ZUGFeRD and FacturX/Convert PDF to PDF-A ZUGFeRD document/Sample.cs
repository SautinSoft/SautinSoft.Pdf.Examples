using System;
using System.IO;
using System.Reflection;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Convert PDF to PDF-A ZUGFeRD using C# and .NET.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-pdf-to-pdfa-zugferd.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");
            string inpFile = @"..\..\..\ZUGFeRD\ZUGFeRD.pdf";
            string outFile = @"..\..\..\ZUGFeRD\ZUGFeRD_Result.pdf";
            string xmlInfo = @"..\..\..\ZUGFeRD\ZUGFeRD.xml";
            // Load a PDF document.
            using (var document = PdfDocument.Load(Path.GetFullPath(inpFile)))
            {
                // Create PDF save options.
                var pdfOptions = new PdfSaveOptions()
                {
                //ZUGFeRD is a German and European standard for hybrid electronic invoices, combining visual PDF and structured XML data. 
				//It uses the ISO 19005-3:2012 (PDF/A-3) standard to embed XML within PDF, enabling long-term storage and automated data processing.
				// Select the desired PDF/A version.
                    Version = PdfVersion.PDF_A_3A,
                    FacturXXml = File.ReadAllText(xmlInfo)
                };

                // Save a PDF document like the FacturX Zugferd.
				// Read more information about Factur-X: https://fnfe-mpe.org/factur-x/

                document.Save(outFile, pdfOptions);
            }
        }
    }
}
