Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Redact
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/redact.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free license:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")

			Dim document = PdfDocument.Load(pdfFile)
			If True Then
				' Assume we want to redact the word "North".
				Dim textToRedact As String = "North"

				Dim page = document.Pages(0)
				Dim texts = page.Content.GetText().Find(textToRedact)

				For Each PdfText In texts
					PdfText.Redact()
					' If you want, draw a green rectangle 
					' at the places where was the text.
					Dim bounds = PdfText.Bounds
					Dim rectangle = page.Content.Elements.AddPath().AddRectangle(New PdfPoint(bounds.Left, bounds.Bottom), New PdfSize(bounds.Width, bounds.Height))
					rectangle.Format.Fill.IsApplied = True
					rectangle.Format.Fill.Color = PdfColor.FromRgb(0, 1, 0)
				Next PdfText
				' Save PDF Document.
				document.Save("out.pdf")
			End If
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("out.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
