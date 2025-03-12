Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Edit PDF files using incremental updates.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/incremental-update.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")
			' Load a PDF document from a file.
			Using document = PdfDocument.Load(pdfFile)
				' Add a page.
				Dim page = document.Pages.Add()

				' Write a text.
				Using formattedText = New PdfFormattedText()
					formattedText.Append("Hello World again!")

					page.Content.DrawText(formattedText, New PdfPoint(100, 700))
				End Using

				' Save all the changes made to the current PDF document using an incremental update.
				document.Save()
				System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(pdfFile) With {.UseShellExecute = True})
			End Using
		End Sub
	End Class
End Namespace
