Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
			''' <summary>
			''' Page parameters.
			''' </summary>
			''' <remarks>
			''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/page-parameters.php
			''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Using document = New PdfDocument()
				Using formattedText = New PdfFormattedText()
					' Get a page tree root node.
					Dim rootNode = document.Pages


					' Create a left page tree node.
					Dim childNode = rootNode.Kids.AddPages()



					' Create a first page.
					Dim page = childNode.Kids.AddPage()
					formattedText.Append("FIRST PAGE")
					page.Content.DrawText(formattedText, New PdfPoint(0, 0))
				End Using

				document.Save("Add Page.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Add Page.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
