using System;
using System.IO;
using System.Reflection.Metadata;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        static void Main(string[] args)
        {

            // Load a PDF document.
            using (var document = PdfDocument.Load(@"..\..\..\simple text.pdf"))
            {  
            // Create image save options.
                var imageOptions = new ImageSaveOptions(ImageSaveFormat.Jpeg)
                {    PageIndex = 0,
                    // PageNumber = 0, // Select the first PDF page.
                    Width = 1240 // Set the image width and keep the aspect ratio.
                };

            // Save a PDF document to a JPEG file.
            document.Save("Output.jpg", imageOptions);
        }
    }
    }
}
