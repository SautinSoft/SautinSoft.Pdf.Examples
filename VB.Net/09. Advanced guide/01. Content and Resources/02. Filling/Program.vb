Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Content

Friend Class Program
	''' <summary>
	''' Filling
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/pdf-content-formatting-filling.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = New PdfDocument()
			Dim page = document.Pages.Add()

			' PdfFormattedText currently supports just Device color spaces (DeviceGray, DeviceRGB, and DeviceCMYK).
			Using formattedText = New PdfFormattedText()
				formattedText.Font = New PdfFont("Helvetica", 100)

				' Make the text fill black (in DeviceGray color space) and 50% opaque.
				formattedText.Color = PdfColor.FromGray(0)
				' In PDF, opacity is defined separately from the color.
				formattedText.Opacity = 0.5
				formattedText.Append("Hello world!")

				page.Content.DrawText(formattedText, New PdfPoint(50, 700))
			End Using

			' Path filled with non-zero winding number rule.
			Dim path = page.Content.Elements.AddPath()
			Dim center = New PdfPoint(300, 500)
			Dim radius As Double = 150, cos1 As Double = Math.Cos(Math.PI / 10), sin1 As Double = Math.Sin(Math.PI / 10), cos2 As Double = Math.Cos(Math.PI / 5), sin2 As Double = Math.Sin(Math.PI / 5)
			' Create a five-point star.
			path.BeginSubpath(center.X - sin2 * radius, center.Y - cos2 * radius).LineTo(center.X + cos1 * radius, center.Y + sin1 * radius).LineTo(center.X - cos1 * radius, center.Y + sin1 * radius).LineTo(center.X + sin2 * radius, center.Y - cos2 * radius).LineTo(center.X, center.Y + radius).CloseSubpath() ' End with the starting point.
			Dim format = path.Format
			format.Fill.IsApplied = True
			format.Fill.Rule = PdfFillRule.NonzeroWindingNumber
			' Make the path fill red (in DeviceRGB color space) and 40% opaque.
			format.Fill.Color = PdfColor.FromRgb(1, 0, 0)
			format.Fill.Opacity = 0.4

			' Path filled with even-odd rule.
			path = page.Content.Elements.AddClone(path)
			path.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -300))
			format = path.Format
			format.Fill.IsApplied = True
			format.Fill.Rule = PdfFillRule.EvenOdd
			' Make the path fill yellow (in DeviceCMYK color space) and 60% opaque.
			format.Fill.Color = PdfColor.FromCmyk(0, 0, 1, 0)
			format.Fill.Opacity = 0.6

			document.Save("Filling.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Filling.pdf") With {.UseShellExecute = True})
	End Sub
End Class
