Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Content

Friend Class Program
	''' <summary>
	''' Clipping.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/pdf-content-formatting-clipping.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free 100-day trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = New PdfDocument()
			Dim page = document.Pages.Add()

			' Add a new content group. Clipping is localized to the content group.
			Dim textGroup = page.Content.Elements.AddGroup()
			' Draw text in the content group.
			Using formattedText = New PdfFormattedText()
				formattedText.Font = New PdfFont("Helvetica", 96)

				formattedText.Append("Hello world!")

				textGroup.DrawText(formattedText, New PdfPoint(50, 700))
			End Using
			' Stroke all text elements in the group (to visualize their bounds) and set them as a clipping path.
			Dim format = textGroup.Format
			format.Fill.IsApplied = False
			format.Stroke.IsApplied = True
			format.Clip.IsApplied = True
			' Draw an image in the same content group as the text.
			' The image will be clipped to the text.
			Dim image = PdfImage.Load("..\..\..\JPEG2.jpg")
			textGroup.DrawImage(image, New PdfPoint(50, 700), New PdfSize(500, 100))

			' Add a new content group. Clipping is localized to the content group.
			Dim pathGroup = page.Content.Elements.AddGroup()
			' Add a diamond-like path to the content group.
			pathGroup.Elements.AddPath().BeginSubpath(50, 550).LineTo(300, 500).LineTo(550, 550).LineTo(300, 600).CloseSubpath()
			' Stroke all path elements in the group (to visualize their bounds) and set them as a clipping path.
			format = pathGroup.Format
			format.Fill.IsApplied = False
			format.Stroke.IsApplied = True
			format.Clip.IsApplied = True
			' Draw an image in the same content group as the diamond-like path.
			' The image will be clipped to the diamond-like path.
			pathGroup.DrawImage(image, New PdfPoint(50, 500), New PdfSize(500, 100))

			' Add a new content group. Clipping is localized to the content group.
			pathGroup = page.Content.Elements.AddGroup()
			' Add a star-like path to the content group.
			Dim path = pathGroup.Elements.AddPath()
			Dim center = New PdfPoint(150, 300)
			Dim radius As Double = 100, cos1 As Double = Math.Cos(Math.PI / 10), sin1 As Double = Math.Sin(Math.PI / 10), cos2 As Double = Math.Cos(Math.PI / 5), sin2 As Double = Math.Sin(Math.PI / 5)
			' Create a five-point star.
			path.BeginSubpath(center.X - sin2 * radius, center.Y - cos2 * radius).LineTo(center.X + cos1 * radius, center.Y + sin1 * radius).LineTo(center.X - cos1 * radius, center.Y + sin1 * radius).LineTo(center.X + sin2 * radius, center.Y - cos2 * radius).LineTo(center.X, center.Y + radius).CloseSubpath() ' End with the starting point.
							' Stroke a path (to visualize its bounds) and set it as a clipping path using non-zero winding number rule.
			format = path.Format
			format.Fill.IsApplied = False
			format.Stroke.IsApplied = True
			format.Clip.IsApplied = True
			format.Clip.Rule = PdfFillRule.NonzeroWindingNumber
			' Draw an image in the same content group as the star-like path.
			' The image will be clipped to the star-like path using non-zero winding number rule.
			pathGroup.DrawImage(image, New PdfPoint(50, 200), New PdfSize(200, 200))

			' Add a new content group. Clipping is localized to the content group.
			pathGroup = page.Content.Elements.AddGroup()
			' Clone a star-like path to the content group and move it down.
			path = pathGroup.Elements.AddClone(path)
			path.Subpaths.Transform(PdfMatrix.CreateTranslation(250, 0))
			' Set the clipping rule to even-odd.
			path.Format.Clip.Rule = PdfFillRule.EvenOdd
			' Draw an image in the same content group as the star-like path.
			' The image will be clipped to the star-like path using the even-odd rule.
			pathGroup.DrawImage(image, New PdfPoint(300, 200), New PdfSize(200, 200))

			document.Save("Clipping.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Clipping.pdf") With {.UseShellExecute = True})
	End Sub
End Class
