using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using System.Linq;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Split PDF files.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/split-pdf-files.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            // Open a source PDF file and create a destination ZIP file.
            using (var source = PdfDocument.Load(Path.GetFullPath(@"..\..\..\simple text.pdf")))
            using (var archiveStream = File.OpenWrite("Output.zip"))
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create))
            {
                // Iterate through the PDF pages.
                for (int pageIndex = 0; pageIndex < source.Pages.Count; pageIndex++)
                {
                    // Create a ZIP entry for each source document page.
                    var entry = archive.CreateEntry($"Page {pageIndex + 1}.pdf");

                    // Save each page as a separate destination document to the ZIP entry.
                    using (var entryStream = entry.Open())
                    using (var destination = new PdfDocument())
                    {
                        destination.Pages.AddClone(source.Pages[pageIndex]);
                        destination.Save(entryStream);
                    }
                }
            }
        }
    }
}
