Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Content

Friend Class Program
	''' <summary>
	''' Stroking
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/pdf-content-formatting-stroking.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = New PdfDocument()
			Dim page = document.Pages.Add()

			' PdfFormattedText currently doesn't support stroking, so we will stroke its drawn output.
			Using formattedText = New PdfFormattedText()
				formattedText.Font = New PdfFont("Helvetica", 200)

				formattedText.Append("Hello!")

				page.Content.DrawText(formattedText, New PdfPoint(50, 600))
			End Using

			' Draw lines with different line joins.
			Dim path = page.Content.Elements.AddPath()
			path.BeginSubpath(50, 350).LineTo(100, 550).LineTo(150, 350)
			path.Format.Stroke.LineJoin = PdfLineJoin.Miter

			path = page.Content.Elements.AddPath()
			path.BeginSubpath(200, 350).LineTo(250, 550).LineTo(300, 350)
			path.Format.Stroke.LineJoin = PdfLineJoin.Round

			path = page.Content.Elements.AddPath()
			path.BeginSubpath(350, 350).LineTo(400, 550).LineTo(450, 350)
			path.Format.Stroke.LineJoin = PdfLineJoin.Bevel

			' Create a dash pattern with 20 point dashes and 10 point gaps.
			Dim dashPattern = New PdfLineDashPattern(0, 20, 10)

			' Draw lines with different line caps.
			' Notice how the line cap is applied to each dash.
			path = page.Content.Elements.AddPath()
			path.BeginSubpath(50, 100).LineTo(100, 300).LineTo(150, 100)
			Dim format = path.Format
			format.Stroke.LineCap = PdfLineCap.Butt
			format.Stroke.DashPattern = dashPattern

			path = page.Content.Elements.AddPath()
			path.BeginSubpath(200, 100).LineTo(250, 300).LineTo(300, 100)
			format = path.Format
			format.Stroke.LineCap = PdfLineCap.Round
			format.Stroke.DashPattern = dashPattern

			path = page.Content.Elements.AddPath()
			path.BeginSubpath(350, 100).LineTo(400, 300).LineTo(450, 100)
			format = path.Format
			format.Stroke.LineCap = PdfLineCap.Square
			format.Stroke.DashPattern = dashPattern

			' Do not fill any content and stroke all content with a 10 point width red outline that is 50% opaque.
			format = page.Content.Format
			format.Fill.IsApplied = False
			format.Stroke.IsApplied = True
			format.Stroke.Width = 10
			format.Stroke.Color = PdfColor.FromRgb(1, 0, 0)
			format.Stroke.Opacity = 0.5

			' Add a line to visualize the differences between line joins.
			Dim line = page.Content.Elements.AddPath().BeginSubpath(25, 550).LineTo(475, 550)
			format = line.Format
			format.Stroke.IsApplied = True
			' A line width of 0 denotes the thinnest line that can be rendered at device resolution: 1 device pixel wide.
			format.Stroke.Width = 0

			' Add a line to visualize the differences between line caps.
			line = page.Content.Elements.AddPath().BeginSubpath(25, 100).LineTo(475, 100)
			format = line.Format
			format.Stroke.IsApplied = True
			format.Stroke.Width = 0

			document.Save("Stroking.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Stroking.pdf") With {.UseShellExecute = True})
	End Sub
End Class
