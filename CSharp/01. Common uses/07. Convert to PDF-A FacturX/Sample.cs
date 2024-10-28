using System;
using System.IO;
using System.Reflection.Metadata;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Convert PDF to PDF/A FacturX using C# and .NET.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-to-pdfa-facturx.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");
            string inpFile = @"..\..\..\Factur.rtf";
            string outFile = @"..\..\..\Factur.pdf";
            string xmlInfo = @"..\..\..\Factur\Facture.xml";
            // Load a PDF document.
            using (var document = PdfDocument.Load(Path.GetFullPath(inpFile)))
            {
                // Create PDF save options.
                var pdfOptions = new PdfSaveOptions()
                {
                // Factur-X is at the same time a full readable invoice in a PDF A/3 format,
                // containing all information useful for its treatment, especially in case of discrepancy or absence of automatic matching with orders and / or receptions,
                // and a set of invoice data presented in an XML structured file conformant to EN16931 (syntax CII D16B), complete or not, allowing invoice process automation.

                    // Select the desired PDF/A version.
                    Version = PdfVersion.FacturX,
                    FacturXXml = File.ReadAllText(xmlInfo)
                };

                // Save a PDF document like the FacturX Zugferd.
				// Read more information about Factur-X: https://fnfe-mpe.org/factur-x/

                document.Save(outFile, pdfOptions);
            }
        }
    }
}
