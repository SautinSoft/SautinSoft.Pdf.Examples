using System;
using System.IO;
using System.Reflection;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Convert PDF to TIFF in C# and .NET.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-pdf-to-multipage-tiff-using-csharp-and-dotnet.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            // Load a PDF document.
            using (var document = PdfDocument.Load(Path.GetFullPath(@"..\..\..\Butterfly.pdf")))
            {
                // Create image save options.
                var tiffOptions = new ImageSaveOptions() 
                { 
                    Format = ImageSaveFormat.Tiff, 
                    // Start from the second page (index 1).
                    PageIndex = 1,
                    PageCount = 3,
                    DpiX = 300,
                    DpiY = 300,
                    PixelFormat = PixelFormat.Rgb24 
                };

                // Save a TIFF file.
                document.Save("Result.tiff", tiffOptions);
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(@"Result.tiff") { UseShellExecute = true });
        }
    }
}
