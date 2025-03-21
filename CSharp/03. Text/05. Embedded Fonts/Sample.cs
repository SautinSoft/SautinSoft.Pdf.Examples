using System;
using System.IO;
using System.Linq;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Using Embedded Fonts
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/embedded-fonts.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            using (var document = new PdfDocument())
            {
                var page = document.Pages.Add();

                using (var formattedText = new PdfFormattedText())
                {
                    formattedText.FontSize = 48;
                    formattedText.LineHeight = 72;

                    // Use the font family 'Almonte Snow' whose font file is located in the 'Resources' directory.
                    formattedText.FontFamily = new PdfFontFamily(@"..\..\..\Resources", "Almonte Snow");
                    formattedText.AppendLine("Hello World 1!");

                    // Use the font family 'Almonte Woodgrain' whose font file is located in the 'Resources' location of the current assembly.
                    formattedText.FontFamily = new PdfFontFamily("..\\..\\..\\Resources", "Almonte Woodgrain");
                    formattedText.AppendLine("Hello World 2!");

                    // Another way to use the font family 'Almonte Snow' whose font file is located in the 'Resources' directory.
                    formattedText.FontFamily = PdfFonts.GetFontFamilies("..\\..\\..\\Resources").First(ff => ff.Name == "Almonte Snow");
                    formattedText.AppendLine("Hello World 3!");

                    // Another way to use the font family 'Almonte Woodgrain' whose font file is located in the 'Resources' location of the current assembly.
                    formattedText.FontFamily = PdfFonts.GetFontFamilies("..\\..\\..\\Resources").First(ff => ff.Name == "Almonte Woodgrain");
                    formattedText.Append("Hello World 4!");

                    // Draw this text.
                    page.Content.DrawText(formattedText, new PdfPoint(100, 500));
                }
                // Save PDF Document.
                document.Save("Private Fonts.pdf");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Private Fonts.pdf") { UseShellExecute = true });
        }
    }
}
