Imports System
Imports System.IO
Imports System.Reflection.Metadata
Imports SautinSoft
Imports SautinSoft.Document
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Annotations
Imports SautinSoft.Pdf.Content

Namespace Sample
    Class Sample
        ''' <summary>
        ''' Merge PDF files and create TOC.
        ''' </summary>
        ''' <remarks>
        ''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/table-of-content.php
        ''' </remarks>
        Shared Sub Main(args As String())
            ' Before starting this example, please get a free 100-day trial key:
            ' https://sautinsoft.com/start-for-free/

            ' Apply the key here:
            ' PdfDocument.SetLicense("...")

            Dim inpFiles As String() = {
                Path.GetFullPath("..\..\..\Simple Text.pdf"),
                Path.GetFullPath("..\..\..\Potato Beetle.pdf"),
                Path.GetFullPath("..\..\..\Text and Graphics.pdf")
            }

            Dim outFile As String = Path.GetFullPath("Merged.pdf")
            Dim tocEntries As New List(Of (Title As String, PagesCount As Integer))

            ' Create a new PDF document.
            Using pdf As New PdfDocument()
                ' Merge multiple PDF documents the new single PDF.
                For Each inpFile In inpFiles
                    Using source As PdfDocument = PdfDocument.Load(inpFile)
                        pdf.Pages.Kids.AddClone(source.Pages)
                        tocEntries.Add((Path.GetFileNameWithoutExtension(inpFile), source.Pages.Count))
                    End Using
                Next

                Dim pagesCount As Integer
                Dim tocPagesCount As Integer

                ' Create PDF with Table of Contents.
                Using tocDocument As PdfDocument = PdfDocument.Load(CreatePdfWithToc(tocEntries))
                    pagesCount = tocDocument.Pages.Count
                    tocPagesCount = pagesCount - tocEntries.Sum(Function(entry) entry.PagesCount)

                    ' Remove empty (placeholder) pages.
                    For i As Integer = pagesCount - 1 To tocPagesCount Step -1
                        tocDocument.Pages.RemoveAt(i)
                    Next

                    ' Insert TOC pages.
                    pdf.Pages.Kids.InsertClone(0, tocDocument.Pages)
                End Using

                Dim entryIndex As Integer = 0
                Dim entryPageIndex As Integer = tocPagesCount

                ' Update TOC links and outlines so that they point to adequate pages instead of placeholder pages.
                For i As Integer = 0 To tocPagesCount - 1
                    For Each annotation In pdf.Pages(i).Annotations.OfType(Of PdfLinkAnnotation)()
                        Dim entryPage As PdfPage = pdf.Pages(entryPageIndex)
                        annotation.SetDestination(entryPage, PdfDestinationViewType.FitPage)

                        entryPageIndex += tocEntries(entryIndex).PagesCount
                        entryIndex += 1
                    Next
                Next

                pdf.Save(outFile)
            End Using

            ' Show the result.
            System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(outFile) With {.UseShellExecute = True})
        End Sub

        Shared Function CreatePdfWithToc(tocEntries As List(Of (Title As String, PagesCount As Integer))) As Stream
            ' Create new document.
            Dim document As New DocumentCore()
            Dim section As New Section(document)
            document.Sections.Add(section)

            ' Add Table of Content.
            Dim toc As New TableOfEntries(document, FieldType.TOC)
            section.Blocks.Add(toc)

            ' Create heading style.
            Dim heading1Style As ParagraphStyle = CType(document.Styles.GetOrAdd(StyleTemplateType.Heading1), ParagraphStyle)
            heading1Style.ParagraphFormat.PageBreakBefore = True

            ' Add headings and placeholder pages.
            For Each entry In tocEntries
                Dim heading As New Paragraph(document)
                heading.ParagraphFormat.Style = heading1Style
                heading.Content.Start.Insert(entry.Title)
                section.Blocks.Add(heading)

                For i As Integer = 1 To entry.PagesCount
                    section.Blocks.Add(New Paragraph(document))
                Next
            Next

            ' Save the document to a memory stream.
            Dim stream As New MemoryStream()
            document.Save(stream, New SautinSoft.Document.PdfSaveOptions())
            stream.Position = 0
            Return stream
        End Function
    End Class
End Namespace


