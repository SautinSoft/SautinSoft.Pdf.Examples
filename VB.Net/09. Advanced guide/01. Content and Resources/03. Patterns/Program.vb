Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Content.Patterns
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Objects
Imports SautinSoft.Pdf.Content.Colors

Friend Class Program
	''' <summary>
	''' Patterns
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/pdf-content-formatting-patterns.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free 100-day trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = New PdfDocument()
				' The uncolored tiling pattern should not specify the color of its content, instead the outer element that uses the uncolored tiling pattern will specify the color of the tiling pattern content.
				Dim uncoloredTilingPattern = New PdfTilingPattern(document, New PdfSize(100, 100)) With {.IsColored = False}
				' Begin editing the pattern cell.
				uncoloredTilingPattern.Content.BeginEdit()
				' The tiling pattern cell contains two triangles that are filled with color specified by the outer element that uses the uncolored tiling pattern.
				Dim path = uncoloredTilingPattern.Content.Elements.AddPath()
				path.BeginSubpath(0, 0).LineTo(50, 0).LineTo(50, 100).CloseSubpath()
				path.Format.Fill.IsApplied = True
				path.BeginSubpath(50, 0).LineTo(100, 0).LineTo(100, 100).CloseSubpath()
				path.Format.Fill.IsApplied = True
				' End editing the pattern cell.
				uncoloredTilingPattern.Content.EndEdit()

				' Create an uncolored tiling Pattern color space.
				' as specified in http://www.adobe.com/content/dam/acom/en/devnet/pdf/PDF32000_2008.pdf#page=186.
				' The underlying color space is DeviceRGB and colorants will be specified in DeviceRGB.
				Dim uncoloredTilingPatternColorSpaceArray = PdfArray.Create(2)
				uncoloredTilingPatternColorSpaceArray.Add(PdfName.Create("Pattern"))
				uncoloredTilingPatternColorSpaceArray.Add(PdfName.Create("DeviceRGB"))
				Dim uncoloredTilingPatternColorSpace = PdfColorSpace.FromArray(uncoloredTilingPatternColorSpaceArray)

				Dim page = document.Pages.Add()

				' Add a background rectangle over the entire page that shows how the tiling pattern, by default, starts from the bottom-left corner of the page.
				Dim mediaBox = page.MediaBox
				Dim backgroundRect = page.Content.Elements.AddPath()
				backgroundRect.AddRectangle(mediaBox.Left, mediaBox.Bottom, mediaBox.Width, mediaBox.Height)
				Dim format = backgroundRect.Format
				format.Fill.IsApplied = True
				format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 0, 0, 0)
				format.Fill.Opacity = 0.2

				' Add a rectangle that is filled with the red (red = 1, green = 0, blue = 0) pattern.
				Dim redRect = page.Content.Elements.AddPath()
				redRect.AddRectangle(75, 575, 200, 100)
				format = redRect.Format
				format.Fill.IsApplied = True
				format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 1, 0, 0)
				format.Stroke.IsApplied = True

				' Add a rectangle that is filled with the same pattern, but this time the pattern's color is green (red = 0, green = 1, blue = 0).
				Dim greenRect = page.Content.Elements.AddClone(redRect)
				greenRect.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -150))
				greenRect.Format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 0, 1, 0)

				' Add a rectangle that is filled with the same pattern, but this time the pattern's color is blue (red = 0, green = 0, blue = 1).
				Dim blueRect = page.Content.Elements.AddClone(greenRect)
				blueRect.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -150))
				blueRect.Format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 0, 0, 1)

				' The colored tiling pattern specifies the color of its content.
				Dim coloredTilingPattern = New PdfTilingPattern(document, New PdfSize(100, 100))
				' Begin editing the pattern cell.
				coloredTilingPattern.Content.BeginEdit()
				' The tiling pattern cell contains two triangles. The first one is filled with the red color and the second one is filled with the green color.
				path = coloredTilingPattern.Content.Elements.AddPath()
				path.BeginSubpath(0, 0).LineTo(50, 0).LineTo(50, 100).CloseSubpath()
				format = path.Format
				format.Fill.IsApplied = True
				format.Fill.Color = PdfColors.Red
				path = coloredTilingPattern.Content.Elements.AddPath()
				path.BeginSubpath(50, 0).LineTo(100, 0).LineTo(100, 100).CloseSubpath()
				format = path.Format
				format.Fill.IsApplied = True
				format.Fill.Color = PdfColors.Green
				' End editing the pattern cell.
				coloredTilingPattern.Content.EndEdit()

				' Add a rectangle that is filled with the colored (red-green) tiling pattern.
				Dim redGreenRect = page.Content.Elements.AddPath()
				redGreenRect.AddRectangle(325, 275, 200, 400)
				format = redGreenRect.Format
				format.Fill.IsApplied = True
				format.Fill.Color = PdfColor.FromPattern(coloredTilingPattern)
				format.Stroke.IsApplied = True

				document.Save("Patterns.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Patterns.pdf") With {.UseShellExecute = True})
	End Sub
End Class
