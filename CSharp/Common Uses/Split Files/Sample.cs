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
        static void Main(string[] args)
        {
            // Open a source PDF file and create a destination ZIP file.
            using (var source = PdfDocument.Load(@"..\..\..\simple text.pdf"))
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
