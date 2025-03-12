Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports System.Linq

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Find a specific text on page #2 in the PDF and show Bounds, Coordinates, Points.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/bounds-and-coordinates.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\sample.pdf")

			Using document = PdfDocument.Load(pdfFile)
				' Page #2:
				Dim page = document.Pages(1)
	
				Dim foundText = page.Content.GetText().Find("Best Beaches:").FirstOrDefault()
				If foundText IsNot Nothing Then
					Console.WriteLine(foundText.Bounds)
				End If
			End Using
		End Sub
	End Class
End Namespace
