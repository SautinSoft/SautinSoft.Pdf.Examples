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
		''' Find text in the PDF.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/find-text.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")

			Dim document = PdfDocument.Load(pdfFile)
			If True Then
				' Find all occurrences of a given text in a pdf file.
				Dim text = document.Pages(0).Content.GetText().Find("the")

				Console.WriteLine("Found " & text.Count() & " elements of this symbol combination.")
			End If
		End Sub
	End Class
End Namespace
