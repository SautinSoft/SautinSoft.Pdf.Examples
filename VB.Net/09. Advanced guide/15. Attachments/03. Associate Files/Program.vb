Option Infer On

Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Content.Marked
Imports System.IO

Friend Class Program
	''' <summary>
	''' Associate Files.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/associate-files.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free 100-day trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = New PdfDocument()
			' Make Attachments panel visible.
			document.PageMode = PdfPageMode.UseAttachments

			Using sourceDocument = PdfDocument.Load(Path.GetFullPath("..\..\..\simple text.pdf"))
				' Import the first page of an 'Simple Text.pdf' document.
				Dim page = document.Pages.AddClone(sourceDocument.Pages(0))

				' Associate the 'Simple Text.docx' file to the imported page as a source file and also add it to the document's embedded files.
				page.AssociatedFiles.Add(PdfAssociatedFileRelationshipType.Source, Path.GetFullPath("..\..\..\simple text.docx"), Nothing, document.EmbeddedFiles)
			End Using

			Using sourceDocument = PdfDocument.Load(Path.GetFullPath("..\..\..\Chart.pdf"))
				' Import the first page of a 'Chart.pdf' document.
				Dim page = document.Pages.AddClone(sourceDocument.Pages(0))

				' Group the content of an imported page and mark it with the 'AF' tag.
				Dim chartContentGroup = page.Content.Elements.Group(page.Content.Elements.First, page.Content.Elements.Last)
				Dim markStart = chartContentGroup.Elements.AddMarkStart(New PdfContentMarkTag(PdfContentMarkTagRole.AF), chartContentGroup.Elements.First)
				chartContentGroup.Elements.AddMarkEnd()

				' Associate the 'Chart.xlsx' to the marked content as a source file and also add it to the document's embedded files.
				' The 'Chart.xlsx' file is associated without using a file system utility code.
				Dim embeddedFile = markStart.AssociatedFiles.AddEmpty(PdfAssociatedFileRelationshipType.Source, Path.GetFullPath("..\..\..\Chart.xlsx"), Nothing, document.EmbeddedFiles).EmbeddedFile
				' Associated file must specify modification date.
				embeddedFile.ModificationDate = File.GetLastWriteTime(Path.GetFullPath("..\..\..\Chart.xlsx"))
				' Associated file stream is not compressed since the source file, 'Chart.xlsx', is already compressed.
				Using fileStream = File.OpenRead(Path.GetFullPath("..\..\..\Chart.xlsx"))
				Using embeddedFileStream = embeddedFile.OpenWrite(compress:= False)
					fileStream.CopyTo(embeddedFileStream)
				End Using
				End Using

				' Associate another file, the 'ChartData.csv', to the marked content as a data file and also add it to the document's embedded files.
				markStart.AssociatedFiles.Add(PdfAssociatedFileRelationshipType.Data, Path.GetFullPath("..\..\..\ChartData.csv"), Nothing, document.EmbeddedFiles)
			End Using

			document.Save("Associated Files.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Associated Files.pdf") With {.UseShellExecute = True})
	End Sub
End Class