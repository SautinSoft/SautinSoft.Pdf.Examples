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
        /// Convert PDF to Jpeg.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-pdf-to-images.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            // Load a PDF document.
            using (var document = PdfDocument.Load(Path.GetFullPath(@"..\..\..\simple text.pdf")))
            {  
                // Create image save options.
                var imageOptions = new ImageSaveOptions(ImageSaveFormat.Jpeg)
                {    
                    PageIndex = 0,
                    // PageNumber = 0, // Select the first PDF page.
                    Width = 1240 // Set the image width and keep the aspect ratio.
                };

                // Save a PDF document to a JPEG file.
                document.Save("Output.jpg", imageOptions);
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(@"Output.jpg") { UseShellExecute = true });
        }
    }
}
