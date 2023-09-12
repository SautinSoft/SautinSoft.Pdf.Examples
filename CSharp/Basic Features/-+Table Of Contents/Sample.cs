using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Annotations;
using SautinSoft.Document;


namespace Sample
{
    class Sample
    {
        static void Main(string[] args)
        {
            string[] files =
        {
            @"..\..\..\MergeFile01.pdf",
            @"..\..\..\MergeFile02.pdf",
            @"..\..\..\MergeFile03.pdf",
            @"..\..\..\MergeFile04.pdf"

        };

            var tocEntries = new List<(string Title, int PagesCount)>();
            using (var document = new PdfDocument())
            {
                // Merge PDF files.
                foreach (var file in files)
                    using (var source = PdfDocument.Load(file))
                    {
                        document.Pages.Kids.AddClone(source.Pages);
                        tocEntries.Add((Path.GetFileNameWithoutExtension(file), source.Pages.Count));
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
                    document.Pages.Kids.InsertClone(0, tocDocument.Pages);
                }

                int entryIndex = 0;
                int entryPageIndex = tocPagesCount;

                // Update TOC links and outlines so that they point to adequate pages instead of placeholder pages.
                for (int i = 0; i < tocPagesCount; i++)
                    foreach (var annotation in document.Pages[i].Annotations.OfType<PdfLinkAnnotation>())
                    {
                        var entryPage = document.Pages[entryPageIndex];
                        annotation.SetDestination(entryPage, PdfDestinationViewType.FitPage);
                        document.Outlines[entryIndex].SetDestination(entryPage, PdfDestinationViewType.FitPage);

                        entryPageIndex += tocEntries[entryIndex].PagesCount;
                        ++entryIndex;
                    }

                document.Save("Merge Files With Toc.pdf");
            }
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

