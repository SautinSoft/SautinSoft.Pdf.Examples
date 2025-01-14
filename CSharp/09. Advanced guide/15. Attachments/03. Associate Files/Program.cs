using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Content.Marked;
using System.IO;

class Program
{
    /// <summary>
    /// Associate Files.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/associate-files.php
    /// </remarks>
    static void Main()
    {
        // Before starting this example, please get a free 100-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        using (var document = new PdfDocument())
        {
            // Make Attachments panel visible.
            document.PageMode = PdfPageMode.UseAttachments;

            using (var sourceDocument = PdfDocument.Load(Path.GetFullPath(@"..\..\..\simple text.pdf")))
            {
                // Import the first page of an 'Simple Text.pdf' document.
                var page = document.Pages.AddClone(sourceDocument.Pages[0]);

                // Associate the 'Simple Text.docx' file to the imported page as a source file and also add it to the document's embedded files.
                page.AssociatedFiles.Add(PdfAssociatedFileRelationshipType.Source, Path.GetFullPath(@"..\..\..\simple text.docx"), null, document.EmbeddedFiles);
            }

            using (var sourceDocument = PdfDocument.Load(Path.GetFullPath(@"..\..\..\Chart.pdf")))
            {
                // Import the first page of a 'Chart.pdf' document.
                var page = document.Pages.AddClone(sourceDocument.Pages[0]);

                // Group the content of an imported page and mark it with the 'AF' tag.
                var chartContentGroup = page.Content.Elements.Group(page.Content.Elements.First, page.Content.Elements.Last);
                var markStart = chartContentGroup.Elements.AddMarkStart(new PdfContentMarkTag(PdfContentMarkTagRole.AF), chartContentGroup.Elements.First);
                chartContentGroup.Elements.AddMarkEnd();

                // Associate the 'Chart.xlsx' to the marked content as a source file and also add it to the document's embedded files.
                // The 'Chart.xlsx' file is associated without using a file system utility code.
                var embeddedFile = markStart.AssociatedFiles.AddEmpty(PdfAssociatedFileRelationshipType.Source, Path.GetFullPath(@"..\..\..\Chart.xlsx"), null, document.EmbeddedFiles).EmbeddedFile;
                // Associated file must specify modification date.
                embeddedFile.ModificationDate = File.GetLastWriteTime(Path.GetFullPath(@"..\..\..\Chart.xlsx"));
                // Associated file stream is not compressed since the source file, 'Chart.xlsx', is already compressed.
                using (var fileStream = File.OpenRead(Path.GetFullPath(@"..\..\..\Chart.xlsx")))
                using (var embeddedFileStream = embeddedFile.OpenWrite(compress: false))
                    fileStream.CopyTo(embeddedFileStream);

                // Associate another file, the 'ChartData.csv', to the marked content as a data file and also add it to the document's embedded files.
                markStart.AssociatedFiles.Add(PdfAssociatedFileRelationshipType.Data, Path.GetFullPath(@"..\..\..\ChartData.csv"), null, document.EmbeddedFiles);
            }

            document.Save("Associated Files.pdf");
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Associated Files.pdf") { UseShellExecute = true });
    }
}