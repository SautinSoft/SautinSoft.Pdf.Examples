using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

class Program
{
    static void Main()
    {
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");
        // Iterate through all PDF pages and through each page's content elements,
        // and retrieve only the text content elements.
        using (var document = PdfDocument.Load(pdfFile))
        {
            foreach (var page in document.Pages)
            {
                var contentEnumerator = page.Content.Elements.All(page.Transform).GetEnumerator();
                while (contentEnumerator.MoveNext())
                {
                    if (contentEnumerator.Current.ElementType == PdfContentElementType.Text)
                    {
                        var textElement = (PdfTextContent)contentEnumerator.Current;
                        var text = textElement.ToString();
                        var font = textElement.Format.Text.Font;
                        var color = textElement.Format.Fill.Color;
                        var bounds = textElement.Bounds;

                        contentEnumerator.Transform.Transform(bounds);
                        // Read the text content element's additional information.
                        Console.WriteLine($"Unicode text: {text}");
                        Console.WriteLine($"Font name: {font.Face.Family.Name}");
                        Console.WriteLine($"Font size: {font.Size}");
                        Console.WriteLine($"Font style: {font.Face.Style}");
                        Console.WriteLine($"Font weight: {font.Face.Weight}");
                        if (color.TryGetRgb(out double red, out double green, out double blue))
                            Console.WriteLine($"Color: Red={red}, Green={green}, Blue={blue}");
                        Console.WriteLine($"Bounds: Left={bounds.Left:0.00}, Bottom={bounds.Bottom:0.00}, Right={bounds.Right:0.00}, Top={bounds.Top:0.00}");
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}