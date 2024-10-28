Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Add pages in document.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/add-page.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free lecense:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Using document = New PdfDocument()
				Using formattedText = New PdfFormattedText()
					' Get a page tree root node.
					Dim rootNode = document.Pages
					' Set page rotation for a whole set of pages.
					rootNode.Rotate = 90

					' Create a left page tree node.
					Dim childNode = rootNode.Kids.AddPages()
					' Overwrite a parent tree node rotation value.
					childNode.Rotate = 0

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