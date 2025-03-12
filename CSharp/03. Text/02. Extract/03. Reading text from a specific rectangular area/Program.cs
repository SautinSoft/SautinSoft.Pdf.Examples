using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

class Program
{
    /// <summary>
    /// Reading text
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/reading-text-from-specific-rectangular-area.php
    /// </remarks>
    static void Main()
    {
        // Before starting this example, please get a free trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");
        var pageIndex = 0;
        double areaLeft = 200, areaRight = 520, areaBottom = 510, areaTop = 720;
        using (var document = PdfDocument.Load(pdfFile))
        {
            // Retrieve first page object.
            var page = document.Pages[pageIndex];
            // Retrieve text content elements that are inside specified area on the first page.
            var contentEnumerator = page.Content.Elements.All(page.Transform).GetEnumerator();
            while (contentEnumerator.MoveNext())
            {
                if (contentEnumerator.Current.ElementType == PdfContentElementType.Text)
                {
                    var textElement = (PdfTextContent)contentEnumerator.Current;
                    var bounds = textElement.Bounds;
                    contentEnumerator.Transform.Transform(bounds);

                    if (bounds.Left > areaLeft && bounds.Right < areaRight &&
                    bounds.Bottom > areaBottom && bounds.Top < areaTop)
                    {
                        // Read the text of an element located in a given area
                        Console.Write(textElement.ToString());
                    }
                }
            }
        }
    }
}