Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Friend Class Program
	''' <summary>
	''' Reading text
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/reading-text-from-specific-rectangular-area.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free license:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")
		Dim pageIndex = 0
		Dim areaLeft As Double = 200, areaRight As Double = 520, areaBottom As Double = 510, areaTop As Double = 720
		Using document = PdfDocument.Load(pdfFile)
			' Retrieve first page object.
			Dim page = document.Pages(pageIndex)
			' Retrieve text content elements that are inside specified area on the first page.
			Dim contentEnumerator = page.Content.Elements.All(page.Transform).GetEnumerator()
			Do While contentEnumerator.MoveNext()
				If contentEnumerator.Current.ElementType = PdfContentElementType.Text Then
					Dim textElement = CType(contentEnumerator.Current, PdfTextContent)
					Dim bounds = textElement.Bounds
					contentEnumerator.Transform.Transform(bounds)

					If bounds.Left > areaLeft AndAlso bounds.Right < areaRight AndAlso bounds.Bottom > areaBottom AndAlso bounds.Top < areaTop Then
						' Read the text of an element located in a given area
						Console.Write(textElement.ToString())
					End If
				End If
			Loop
		End Using
	End Sub
End Class
