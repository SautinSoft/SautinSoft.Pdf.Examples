using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Sample
{
    class Program
    {
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/create-pdf.php
        /// </remarks>
        static void Main()
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            using (var document = new PdfDocument())

            {
                var page = document.Pages.Add();

                var formattedText1 = new PdfFormattedText();
                var text1 = "Hello World";
                formattedText1.FontSize = 15;
                formattedText1.FontFamily = new PdfFontFamily("Calibri");
                formattedText1.Append(text1);
                page.Content.DrawText(formattedText1, new PdfPoint(110, 650));

                var formattedText2 = new PdfFormattedText();
                var text2 = "This message was";
                formattedText2.FontSize = 16;
                formattedText2.FontFamily = new PdfFontFamily("Times New Roman");
                formattedText2.FontStyle = PdfFontStyle.Italic;
                formattedText2.Color = PdfColor.FromRgb(1, 0, 0);
                formattedText2.Append(text2);
                page.Content.DrawText(formattedText2, new PdfPoint(115, 632));
                
                var formattedText3 = new PdfFormattedText();
                var text3 = "created by SautinSoft";
                formattedText3.FontSize = 22;
                formattedText3.FontStyle = PdfFontStyle.Italic;
                formattedText3.FontFamily = new PdfFontFamily("Times New Roman");
                formattedText3.Color = PdfColor.FromRgb(1, 0, 0);
                formattedText3.Append(text3);
                page.Content.DrawText(formattedText3, new PdfPoint(110, 610));
                
                var formattedText4 = new PdfFormattedText();
                var text4 = "component!";
                formattedText4.FontSize = 22;
                formattedText4.FontFamily = new PdfFontFamily("Times New Roman");
                formattedText4.Append(text4);
                page.Content.DrawText(formattedText4, new PdfPoint(303, 610));



                document.Save("Output.pdf");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Output.pdf") { UseShellExecute = true });
        }
    }
}
      
    

    