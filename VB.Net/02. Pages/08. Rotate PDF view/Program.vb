Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Content
Imports System.Drawing
Imports System.Runtime.CompilerServices

Namespace Sample
	Friend Class Program
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/rotate-pdf.php
		''' </remarks>
		Shared Sub Main()
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Using document = New PdfDocument()
				Dim page = document.Pages.Add()
				' Rotate page content
				page.Rotate = 0
				' Rotate page view: Landscape or Portrait.
				page.SetMediaBox(page.Size.Height, page.Size.Width)
				Dim formattedText1 = New PdfFormattedText()
				Dim text1 = "Hello World"
				formattedText1.FontSize = 15
				formattedText1.FontFamily = New PdfFontFamily("Calibri")
				formattedText1.Append(text1)
				page.Content.DrawText(formattedText1, New PdfPoint(200, 400))
				document.Save("Output.pdf")
			End Using
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Output.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
