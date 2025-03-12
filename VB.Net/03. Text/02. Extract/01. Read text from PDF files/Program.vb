Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Read text from PDF.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/read-text-from-pdf-files.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")

			' Load PDF Document.
			Using document = PdfDocument.Load(pdfFile)
				For Each page In document.Pages
					' Write text from pdf file to console.
					Console.WriteLine(page.Content.ToString())
				Next page
			End Using
		End Sub
	End Class
End Namespace
