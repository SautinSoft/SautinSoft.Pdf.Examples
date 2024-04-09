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
        /// Get and set document properties.
        /// </summary>
        /// <remarks>
        /// Details: http://sautinsoft/products/pdf/help/net/developer-guide/document-properties-pdf.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                // Get document properties.
                var info = document.Info;

                // Update document properties.
                info.Title = "My Title";
                info.Author = "My Author";
                info.Subject = "My Subject";
                info.Creator = "My Application";

                // Update producer and date information, and disable their overriding.
                info.Producer = "My Producer";
                info.CreationDate = new DateTime(2023, 1, 1, 12, 0, 0);
                info.ModificationDate = new DateTime(2023, 1, 1, 12, 0, 0);
                document.SaveOptions.UpdateProducerInformation = false;
                document.SaveOptions.UpdateDateInformation = false;

                document.Save("Document Properties.pdf");
            }
        }
    }
}
