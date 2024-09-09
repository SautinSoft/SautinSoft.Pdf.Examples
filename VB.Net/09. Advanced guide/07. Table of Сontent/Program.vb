Option Infer On

Imports System
Imports System.IO
Imports System.Reflection.Metadata
Imports SautinSoft
Imports SautinSoft.Document
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Annotations
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Merge PDF files and create TOC.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/table-of-content.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim inpFiles() As String = { Path.GetFullPath("..\..\..\Simple Text.pdf"), Path.GetFullPath("..\..\..\Potato Beetle.pdf"), Path.GetFullPath("..\..\..\Text and Graphics.pdf")}

			Dim outFile As String = Path.GetFullPath("Merged.pdf")
			Dim tocEntries = New List(Of (Title As String, PagesCount As Integer))()
			' Create a new PDF document.
			Using pdf = New PdfDocument()
				' Merge multiple PDF documents the new single PDF.
				For Each inpFile In inpFiles
					Using source = PdfDocument.Load(inpFile)
						pdf.Pages.Kids.AddClone(source.Pages)
						tocEntries.Add((Path.GetFileNameWithoutExtension(inpFile), source.Pages.Count))
					End Using
				Next inpFile

				Dim pagesCount As Integer
				Dim tocPagesCount As Integer

				' Create PDF with Table of Contents.
				Using tocDocument = PdfDocument.Load(CreatePdfWithToc(tocEntries))
					pagesCount = tocDocument.Pages.Count
					tocPagesCount = pagesCount - tocEntries.Sum(Function(entry) entry.PagesCount)

					' Remove empty (placeholder) pages.
					For i As Integer = pagesCount - 1 To tocPagesCount Step -1
						tocDocument.Pages.RemoveAt(i)
					Next i

					' Insert TOC pages.
					pdf.Pages.Kids.InsertClone(0, tocDocument.Pages)
				End Using

				Dim entryIndex As Integer = 0
				Dim entryPageIndex As Integer = tocPagesCount

				' Update TOC links and outlines so that they point to adequate pages instead of placeholder pages.
				For i As Integer = 0 To tocPagesCount - 1
					For Each annotation In pdf.Pages(i).Annotations.OfType(Of PdfLinkAnnotation)()
						Dim entryPage = pdf.Pages(entryPageIndex)
						annotation.SetDestination(entryPage, PdfDestinationViewType.FitPage)

						entryPageIndex += tocEntries(entryIndex).PagesCount
						entryIndex += 1
					Next annotation
				Next i

				pdf.Save(outFile)
			End Using
			' Show the result.
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(outFile) With {.UseShellExecute = True})
		End Sub

		Private Shared Function CreatePdfWithToc(ByVal tocEntries As List(Of (Title As String, PagesCount As Integer))) As Stream
			' Create new document.
			Dim document = New DocumentCore()
			Dim section As New Section(document)
			document.Sections.Add(section)

			' Add Table of Content.
			Dim toc = New TableOfEntries(document, FieldType.TOC)
			section.Blocks.Add(toc)

			' Create heading style.
			Dim heading1Style = CType(document.Styles.GetOrAdd(StyleTemplateType.Heading1), ParagraphStyle)
			heading1Style.ParagraphFormat.PageBreakBefore = True

			' Add heading paragraphs and empty (placeholder) pages.
			For Each tocEntry In tocEntries
				section.Blocks.Add(New Paragraph(document, tocEntry.Title) With {
					.ParagraphFormat = { Style = heading1Style }
				})

				For i As Integer = 0 To tocEntry.PagesCount - 1
					section.Blocks.Add(New Paragraph(document, New SpecialCharacter(document, SpecialCharacterType.PageBreak)))
				Next i
			Next tocEntry

			' Remove last extra-added empty page.
			section.Blocks.RemoveAt(section.Blocks.Count - 1)

			' When updating TOC element, an entry is created for each paragraph that has heading style.
			' The entries have the correct page numbers because of the added placeholder pages.
			toc.Update()

			' Save document as PDF.
			Dim pdfStream = New MemoryStream()
			document.Save(pdfStream, New SautinSoft.Document.PdfSaveOptions())
			Return pdfStream
		End Function
	End Class
End Namespace
