Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/open-pdf.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free license:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")

			Using document = PdfDocument.Load(pdfFile)
				If document IsNot Nothing Then
					For Each page In document.Pages
						Console.WriteLine(page.Content.ToString())
					Next page
					Console.WriteLine("File opened successfully")
				End If
			End Using
		End Sub
	End Class
End Namespace
