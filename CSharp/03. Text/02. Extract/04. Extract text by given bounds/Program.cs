using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

class Program
{
    /// <summary>
    /// How to extract text by given bounds.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/extract-text-from-pdf-by-given-bounds.php
    /// </remarks>
    static void Main()
    {
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");
        string inpFile = Path.GetFullPath(@"..\..\..\extract-text.pdf");

        using (var document = PdfDocument.Load(inpFile))
        {
            // Get the page from which we want to make the extraction
            var page = document.Pages[0];

            // NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
            // and the positive y axis extends vertically upward.
            var pageBounds = page.CropBox;

            // Extract text content from the given bounds
            var text = page.Content.GetText(new PdfTextOptions
            {
                Bounds = new PdfQuad(
                    new PdfPoint(20, pageBounds.Top - 20),
                    new PdfPoint(pageBounds.Right, pageBounds.Top - 20),
                    new PdfPoint(pageBounds.Right, pageBounds.Top - 120),
                    new PdfPoint(20, pageBounds.Top - 120)),
                Order = PdfTextOrder.Reading
            }).ToString();

            // Writing the extracted text
            Console.WriteLine($"Result: {text}");
            Console.ReadKey();
        }
    }
}