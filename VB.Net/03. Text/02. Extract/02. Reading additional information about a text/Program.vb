Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Friend Class Program
	''' <summary>
	''' Reading additional info.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/reading-additional-information.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Dim pdfFile As String = Path.GetFullPath("..\..\..\table.pdf")
		' Iterate through all PDF pages and through each page's content elements,
		' and retrieve only the text content elements.
		Using document = PdfDocument.Load(pdfFile)
			For Each page In document.Pages
				Dim contentEnumerator = page.Content.Elements.All(page.Transform).GetEnumerator()
				Do While contentEnumerator.MoveNext()
					If contentEnumerator.Current.ElementType = PdfContentElementType.Text Then
						Dim textElement = CType(contentEnumerator.Current, PdfTextContent)
						Dim text = textElement.ToString()
						Dim font = textElement.Format.Text.Font
						Dim color = textElement.Format.Fill.Color
						Dim bounds = textElement.Bounds

						contentEnumerator.Transform.Transform(bounds)
						' Read the text content element's additional information.
						Console.WriteLine($"Unicode text: {text}")
						Console.WriteLine($"Font name: {font.Face.Family.Name}")
						Console.WriteLine($"Font size: {font.Size}")
						Console.WriteLine($"Font style: {font.Face.Style}")
						Console.WriteLine($"Font weight: {font.Face.Weight}")
						Dim red As Double
						Dim green As Double
						Dim blue As Double
						If color.TryGetRgb(red, green, blue) Then
							Console.WriteLine($"Color: Red={red}, Green={green}, Blue={blue}")
						End If
						Console.WriteLine($"Bounds: Left={bounds.Left:0.00}, Bottom={bounds.Bottom:0.00}, Right={bounds.Right:0.00}, Top={bounds.Top:0.00}")
						Console.WriteLine()
					End If
				Loop
			Next page
		End Using
	End Sub
End Class
