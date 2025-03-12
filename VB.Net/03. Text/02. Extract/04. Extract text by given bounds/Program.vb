Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Friend Class Program
	''' <summary>
	''' How to extract text by given bounds.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/extract-text-from-pdf-by-given-bounds.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");
		Dim inpFile As String = Path.GetFullPath("..\..\..\extract-text.pdf")

		Using document = PdfDocument.Load(inpFile)
			' Get the page from which we want to make the extraction
			Dim page = document.Pages(0)

			' NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
			' and the positive y axis extends vertically upward.
			Dim pageBounds = page.CropBox

			' Extract text content from the given bounds
			Dim text = page.Content.GetText(New PdfTextOptions With {
				.Bounds = New PdfQuad(New PdfPoint(20, pageBounds.Top - 20), New PdfPoint(pageBounds.Right, pageBounds.Top - 20), New PdfPoint(pageBounds.Right, pageBounds.Top - 120), New PdfPoint(20, pageBounds.Top - 120)),
				.Order = PdfTextOrder.Reading
			})

			' Writing the extracted text
			Console.WriteLine($"Result: {text}")
			Console.WriteLine($"Text position: " & $"(X: {text.Bounds.Left:0.##}, " & $"Y: {text.Bounds.Bottom:0.##}), " & $"Width: {text.Bounds.Width:0.##}, " & $"Height: {text.Bounds.Height:0.##}.")
			Console.ReadKey()
		End Using
	End Sub
End Class
