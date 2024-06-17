using System;
using System.IO;
using System.Reflection.Metadata;
using SautinSoft;
using SautinSoft.Document;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Annotations;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Merge PDF files and create TOC.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/table-of-content.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");
            
            string[] inpFiles = new string[] {
                        Path.GetFullPath(@"..\..\..\Simple Text.pdf"),
                        Path.GetFullPath(@"..\..\..\Potato Beetle.pdf"),
                        Path.GetFullPath(@"..\..\..\Text and Graphics.pdf")};

            string outFile = Path.GetFullPath(@"Merged.pdf");
            var tocEntries = new List<(string Title, int PagesCount)>();
            // Create a new PDF document.
            using (var pdf = new PdfDocument())
            {
                // Merge multiple PDF documents the new single PDF.
                foreach (var inpFile in inpFiles)
                    using (var source = PdfDocument.Load(inpFile))
                    {
                        pdf.Pages.Kids.AddClone(source.Pages);
                        tocEntries.Add((Path.GetFileNameWithoutExtension(inpFile), source.Pages.Count));
                    }

                int pagesCount;
                int tocPagesCount;

                // Create PDF with Table of Contents.
                using (var tocDocument = PdfDocument.Load(CreatePdfWithToc(tocEntries)))
                {
                    pagesCount = tocDocument.Pages.Count;
                    tocPagesCount = pagesCount - tocEntries.Sum(entry => entry.PagesCount);

                    // Remove empty (placeholder) pages.
                    for (int i = pagesCount - 1; i >= tocPagesCount; i--)
                        tocDocument.Pages.RemoveAt(i);

                    // Insert TOC pages.
                    pdf.Pages.Kids.InsertClone(0, tocDocument.Pages);
                }

                int entryIndex = 0;
                int entryPageIndex = tocPagesCount;

                // Update TOC links and outlines so that they point to adequate pages instead of placeholder pages.
                for (int i = 0; i < tocPagesCount; i++)
                    foreach (var annotation in pdf.Pages[i].Annotations.OfType<PdfLinkAnnotation>())
                    {
                        var entryPage = pdf.Pages[entryPageIndex];
                        annotation.SetDestination(entryPage, PdfDestinationViewType.FitPage);

                        entryPageIndex += tocEntries[entryIndex].PagesCount;
                        ++entryIndex;
                    }

                pdf.Save(outFile);
            }
            // Show the result.
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outFile) { UseShellExecute = true });
        }

        static Stream CreatePdfWithToc(List<(string Title, int PagesCount)> tocEntries)
        {
            // Create new document.
            var document = new DocumentCore();
            var section = new Section(document);
            document.Sections.Add(section);

            // Add Table of Content.
            var toc = new TableOfEntries(document, FieldType.TOC);
            section.Blocks.Add(toc);

            // Create heading style.
            var heading1Style = (ParagraphStyle)document.Styles.GetOrAdd(StyleTemplateType.Heading1);
            heading1Style.ParagraphFormat.PageBreakBefore = true;

            // Add heading paragraphs and empty (placeholder) pages.
            foreach (var tocEntry in tocEntries)
            {
                section.Blocks.Add(
                    new Paragraph(document, tocEntry.Title)
                    { ParagraphFormat = { Style = heading1Style } });

                for (int i = 0; i < tocEntry.PagesCount; i++)
                    section.Blocks.Add(
                        new Paragraph(document,
                            new SpecialCharacter(document, SpecialCharacterType.PageBreak)));
            }

            // Remove last extra-added empty page.
            section.Blocks.RemoveAt(section.Blocks.Count - 1);

            // When updating TOC element, an entry is created for each paragraph that has heading style.
            // The entries have the correct page numbers because of the added placeholder pages.
            toc.Update();

            // Save document as PDF.
            var pdfStream = new MemoryStream();
            document.Save(pdfStream, new SautinSoft.Document.PdfSaveOptions());
            return pdfStream;
        }
    }
}