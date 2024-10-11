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
            /// Page parameters.
            /// </summary>
            /// <remarks>
            /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/page-parameters.php
            /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            using (var document = new PdfDocument())
            {
                using (var formattedText = new PdfFormattedText())
                {
                    // Get a page tree root node.
                    var rootNode = document.Pages;
                  
                    // Create a left page tree node.
                    var childNode = rootNode.Kids.AddPages();
                    
                    // Create a first page.
                    var page = childNode.Kids.AddPage();
                    Console.WriteLine("Page rotation: {0}", page.Rotate);
                    Console.WriteLine("Page size: width = {0}, height = {1}", page.Size.Width, page.Size.Height);
                    Console.WriteLine("Page cropBox rectangle: ({0}, {1}, {2}, {3})", page.CropBox.Left, page.CropBox.Bottom, page.CropBox.Right, page.CropBox.Top);
                    Console.WriteLine("Page mediaBox rectangle: ({0}, {1}, {2}, {3})", page.MediaBox.Left, page.MediaBox.Bottom, page.MediaBox.Right, page.MediaBox.Top);
                }
            }
        }
    }
}