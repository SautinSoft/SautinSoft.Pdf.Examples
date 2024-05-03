using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content;

class Program
{
    static void Main()
    {
		/// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/create-pdf.php
        /// </remarks>
		
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        PdfDocument document = new PdfDocument();

        if (document != null)
        {
            Console.WriteLine("File created successfully!");
        }
    }
}