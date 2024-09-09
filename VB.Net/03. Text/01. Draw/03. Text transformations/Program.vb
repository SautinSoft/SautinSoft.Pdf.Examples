Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Content

Friend Class Program
	''' <summary>
	''' Text transformation and rotation.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/text-transformations.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free 100-day trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...")

		Using document = New PdfDocument()
			'Create a new page.
			Dim page = document.Pages.Add()

			Using formattedText = New PdfFormattedText()
				' Set up and fill the PdfFormattedText object.
				Dim text = "Rotated by 30 degrees around origin."
				formattedText.Opacity = 0.2
				formattedText.Append(text)
				Dim origin = New PdfPoint(50, 650)
				' Draw this text.
				page.Content.DrawText(formattedText, origin)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.Opacity = 1
				formattedText.Append(text)
				' Create a trasformation matrix.
				Dim transform = PdfMatrix.Identity
				transform.Translate(origin.X, origin.Y)
				transform.Rotate(30)
				' Draw this text using a transformation matrix.
				page.Content.DrawText(formattedText, transform)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				text = "Rotated by 30 degrees around center."
				formattedText.Opacity = 0.2
				formattedText.Append(text)
				origin = New PdfPoint(300, 650)
				' Draw this text.
				page.Content.DrawText(formattedText, origin)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.Opacity = 1
				formattedText.Append(text)
				' Create a trasformation matrix.
				transform = PdfMatrix.Identity
				transform.Translate(origin.X, origin.Y)
				transform.Rotate(30, formattedText.Width / 2, formattedText.Height / 2)
				' Draw this text using a transformation matrix.
				page.Content.DrawText(formattedText, transform)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				text = "Scaled horizontally by 0.5 around origin."
				formattedText.Opacity = 0.2
				formattedText.Append(text)
				origin = New PdfPoint(50, 500)
				' Draw this text.
				page.Content.DrawText(formattedText, origin)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.Opacity = 1
				formattedText.Append(text)
				' Create a trasformation matrix.
				transform = PdfMatrix.Identity
				transform.Translate(origin.X, origin.Y)
				transform.Scale(0.5, 1)
				' Draw this text using a transformation matrix.
				page.Content.DrawText(formattedText, transform)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				text = "Scaled horizontally by 0.5 around center."
				formattedText.Opacity = 0.2
				formattedText.Append(text)
				origin = New PdfPoint(300, 500)
				' Draw this text.
				page.Content.DrawText(formattedText, origin)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.Opacity = 1
				formattedText.Append(text)
				' Create a trasformation matrix.
				transform = PdfMatrix.Identity
				transform.Translate(origin.X, origin.Y)
				transform.Scale(0.5, 1, formattedText.Width / 2, formattedText.Height / 2)
				' Draw this text using a transformation matrix.
				page.Content.DrawText(formattedText, transform)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				text = "Scaled vertically by 2 around origin."
				formattedText.Opacity = 0.2
				formattedText.Append(text)
				origin = New PdfPoint(50, 400)
				' Draw this text.
				page.Content.DrawText(formattedText, origin)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.Opacity = 1
				formattedText.Append(text)
				' Create a trasformation matrix.
				transform = PdfMatrix.Identity
				transform.Translate(origin.X, origin.Y)
				transform.Scale(1, 2)
				' Draw this text using a transformation matrix.
				page.Content.DrawText(formattedText, transform)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				text = "Scaled vertically by 2 around center."
				formattedText.Opacity = 0.2
				formattedText.Append(text)
				origin = New PdfPoint(300, 400)
				' Draw this text.
				page.Content.DrawText(formattedText, origin)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.Opacity = 1
				formattedText.Append(text)
				' Create a trasformation matrix.
				transform = PdfMatrix.Identity
				transform.Translate(origin.X, origin.Y)
				transform.Scale(1, 2, formattedText.Width / 2, formattedText.Height / 2)
				' Draw this text using a transformation matrix.
				page.Content.DrawText(formattedText, transform)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				text = "Rotated by 30 degrees around origin and "
				Dim text2 = "scaled horizontally by 0.5 and "
				Dim text3 = "vertically by 2 around origin."
				formattedText.Opacity = 0.2
				formattedText.AppendLine(text).AppendLine(text2).Append(text3)
				origin = New PdfPoint(50, 200)
				' Draw this text.
				page.Content.DrawText(formattedText, origin)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.Opacity = 1
				formattedText.AppendLine(text).AppendLine(text2).Append(text3)
				' Create a trasformation matrix.
				transform = PdfMatrix.Identity
				transform.Translate(origin.X, origin.Y)
				transform.Rotate(30)
				transform.Scale(0.5, 2)
				' Draw this text using a transformation matrix.
				page.Content.DrawText(formattedText, transform)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				text = "Rotated by 30 degrees around center and "
				text2 = "scaled horizontally by 0.5 and "
				text3 = "vertically by 2 around center."
				formattedText.Opacity = 0.2
				formattedText.AppendLine(text).AppendLine(text2).Append(text3)
				origin = New PdfPoint(300, 200)
				' Draw this text.
				page.Content.DrawText(formattedText, origin)
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.Opacity = 1
				formattedText.AppendLine(text).AppendLine(text2).Append(text3)
				' Create a trasformation matrix.
				transform = PdfMatrix.Identity
				transform.Translate(origin.X, origin.Y)
				transform.Rotate(30, formattedText.Width / 2, formattedText.Height / 2)
				transform.Scale(0.5, 2, formattedText.Width / 2, formattedText.Height / 2)
				' Draw this text using a transformation matrix.
				page.Content.DrawText(formattedText, transform)
			End Using
			' Save PDF Document.
			document.Save("Transformations.pdf")
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Transformations.pdf") With {.UseShellExecute = True})
		End Using
	End Sub
End Class
