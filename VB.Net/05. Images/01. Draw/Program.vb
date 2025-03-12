Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Content

Friend Class Program
	''' <summary>
	''' Add shapes to PDF files.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/draw-images-to-pdf.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = New PdfDocument()
			' Add a page.
			Dim page = document.Pages.Add()

			' Load the image from a file.
			Dim image = PdfImage.Load("..\..\..\parrot.png")

			' Set the location of the bottom-left corner of the image.
			' We want the top-left corner of the image to be at location (50, 50)
			' from the top-left corner of the page.
			' NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
			' and the positive y axis extends vertically upward.
			Dim x As Double = 50, y As Double = page.CropBox.Top - 50 - image.Size.Height

			' Draw the image to the page.
			page.Content.DrawImage(image, New PdfPoint(x, y))

			document.Save("Parrot.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Parrot.pdf") With {.UseShellExecute = True})
	End Sub
End Class
