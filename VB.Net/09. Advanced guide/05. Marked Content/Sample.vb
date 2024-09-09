Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Content.Marked
Imports SautinSoft.Pdf.Objects

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Create PDF marked content.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/marked-content.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Using document = New PdfDocument()
				Dim page = document.Pages.Add()

				' Surround the path with the marked content start and marked content end elements.
				Dim markStart = page.Content.Elements.AddMarkStart(New PdfContentMarkTag(PdfContentMarkTagRole.Span))

				Dim markedProperties = markStart.GetEditableProperties().GetDictionary()

				' Add the path that is a visual representation of the letter 'H'.
				Dim path = page.Content.Elements.AddPath().BeginSubpath(100, 600).LineTo(100, 800).BeginSubpath(100, 700).LineTo(200, 700).BeginSubpath(200, 600).LineTo(200, 800)

				Dim format = path.Format
				format.Stroke.IsApplied = True
				format.Stroke.Width = 10

				page.Content.Elements.AddMarkEnd()

				document.Save("MarkedContent.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("MarkedContent.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
