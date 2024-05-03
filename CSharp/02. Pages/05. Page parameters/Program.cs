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
            /// Create a page tree.
            /// </summary>
            /// <remarks>
            /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/page-parameters.php
            /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            using (var document = new PdfDocument())
            {
                using (var formattedText = new PdfFormattedText())
                {
                    // Get a page tree root node.
                    var rootNode = document.Pages;
                    // Set page rotation for a whole set of pages.
                    rootNode.Rotate = 90;

                    // Create a left page tree node.
                    var childNode = rootNode.Kids.AddPages();
                    // Overwrite a parent tree node rotation value.
                    childNode.Rotate = 180;
                    childNode.SetCropBox(400, 600);
                    childNode.SetMediaBox(400, 600);


                    // Create a first page.
                    var page = childNode.Kids.AddPage();
                    formattedText.Append("FIRST PAGE");
                    page.Content.DrawText(formattedText, new PdfPoint(0, 0));
                }

                document.Save("Add Page.pdf");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Add Page.pdf") { UseShellExecute = true });
        }
    }
}