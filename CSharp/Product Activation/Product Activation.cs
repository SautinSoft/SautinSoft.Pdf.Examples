using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        static void Main(string[] args)
        {
            // Get your free 30-day key here:   
            // https://sautinsoft.com/start-for-free/

            ProductActivation();
        }

        /// <summary>
        /// PDF .Net activation.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/getting-started/product-activation.php
        /// </remarks>
        static void ProductActivation()
        {
            // PDF .Net activation.

            // You will get own serial number after purchasing the license.
            // If you will have any questions, email us to sales@sautinsoft.com or ask at online chat https://www.sautinsoft.com.
            SautinSoft.Pdf.PdfDocument.SetLicense("1234567890");
            
            // NOTICE: Place this line firstly, before creating of the PDF object.
            string inpFile = @"..\..\..\example.pdf";
            string outFile = @"Result.pdf";

            using (var pdf = PdfDocument.Load(inpFile))
            {
                // Add a page.
                var page = pdf.Pages.Add();

                // Write a text.
                using (var formattedText = new PdfFormattedText())
                {
                    formattedText.Append("Hello World!");

                    page.Content.DrawText(formattedText, new PdfPoint(100, 700));
                }

                // Save all the changes made to the current PDF document using an incremental update.
                pdf.Save(outFile);

                // Open the result for demonstration purposes.
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outFile) { UseShellExecute = true });

            }
        }
    }
}
