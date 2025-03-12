Option Infer On

Imports System
Imports System.IO
Imports System.Linq
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Export and import images to PDF file.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/extract-images-from-pdf.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")

			Using document = PdfDocument.Load(pdfFile)
				' Iterate through PDF pages.
				For Each page In document.Pages
					' Get all image content elements on the page.
					Dim imageElements = page.Content.Elements.All().OfType(Of PdfImageContent)().ToList()

					' Export the first image element to an image file.
					If imageElements.Count > 0 Then
						imageElements(0).Save("Export Images.jpeg")
						System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Export Images.jpeg") With {.UseShellExecute = True})
						Exit For
					End If
				Next page
			End Using
		End Sub
	End Class
End Namespace
