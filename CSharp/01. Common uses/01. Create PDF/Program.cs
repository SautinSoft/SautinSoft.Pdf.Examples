using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content;

class Program
{
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/create-pdf.php
    /// </remarks>
    static void Main()
    {
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

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