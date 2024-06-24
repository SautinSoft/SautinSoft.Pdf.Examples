using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using System;
using static System.Collections.Specialized.BitVector32;

namespace Sample
{
    class Sample
    {
        static void Main(string[] args)
        {
            // Get your free 100-day key here:   
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

            string serial = "1234567890";           

            // NOTICE: Place this line firstly, before creating of the PdfDocument object.
              PdfDocument.SetLicense(serial);

            using (var document = new PdfDocument())
            {
                using (var formattedText = new PdfFormattedText())
                {
                    var page = document.Pages.Add();
                    formattedText.Append("Hello from SautinSoft");
                    page.Content.DrawText(formattedText, new PdfPoint(250, 330));
                }
                document.Save("Output.pdf");
            }
        }
    }
}