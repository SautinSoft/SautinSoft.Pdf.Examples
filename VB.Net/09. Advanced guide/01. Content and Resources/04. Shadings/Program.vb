Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Objects
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Content.Patterns

Friend Class Program
	''' <summary>
	''' Shadings.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/pdf-content-formatting-shadings.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = New PdfDocument()
			' Create axial shading
			' as specified in http://www.adobe.com/content/dam/acom/en/devnet/pdf/PDF32000_2008.pdf#page=193
			Dim shadingDictionary = PdfDictionary.Create()
			PdfIndirectObject.Create(shadingDictionary)
			shadingDictionary(PdfName.Create("ShadingType")) = PdfInteger.Create(2)
			' Color values of the shading will be expressed in DeviceRGB color space.
			shadingDictionary(PdfName.Create("ColorSpace")) = PdfName.Create("DeviceRGB")
			' Shading transitions the colors in the axis from location (0, 0) to location (250, 250).
			shadingDictionary(PdfName.Create("Coords")) = PdfArray.Create(PdfNumber.Create(0), PdfNumber.Create(0), PdfNumber.Create(250), PdfNumber.Create(250))
			Dim functionDictionary = PdfDictionary.Create()
			functionDictionary(PdfName.Create("FunctionType")) = PdfInteger.Create(2)
			functionDictionary(PdfName.Create("Domain")) = PdfArray.Create(PdfNumber.Create(0), PdfNumber.Create(1))
			functionDictionary(PdfName.Create("N")) = PdfNumber.Create(1)
			' Red color transitions from 1 (C0[0]) to 0 (C1[0]).
			' Green color transitions from 0 (C0[1]) to 1 (C1[1]).
			' Blue color is always 0 (C0[2] and C1[2] are 0).
			functionDictionary(PdfName.Create("C0")) = PdfArray.Create(PdfNumber.Create(1), PdfNumber.Create(0), PdfNumber.Create(0))
			functionDictionary(PdfName.Create("C1")) = PdfArray.Create(PdfNumber.Create(0), PdfNumber.Create(1), PdfNumber.Create(0))
			shadingDictionary(PdfName.Create("Function")) = functionDictionary
			Dim shading = PdfShading.FromDictionary(shadingDictionary)

			Dim shadingPattern = New PdfShadingPattern(document, shading)

			Dim page = document.Pages.Add()

			' Add a background rectangle over the entire page that shows how the shading pattern, by default, starts from the bottom-left corner of the page.
			Dim mediaBox = page.MediaBox
			Dim backgroundRect = page.Content.Elements.AddPath()
			backgroundRect.AddRectangle(mediaBox.Left, mediaBox.Bottom, mediaBox.Width, mediaBox.Height)
			Dim format = backgroundRect.Format
			format.Fill.IsApplied = True
			format.Fill.Color = PdfColor.FromPattern(shadingPattern)
			format.Fill.Opacity = 0.2

			' Add a square that is filled with the shading pattern.
			Dim square = page.Content.Elements.AddPath()
			square.AddRectangle(25, 25, 200, 200)
			format = square.Format
			format.Fill.IsApplied = True
			format.Fill.Color = PdfColor.FromPattern(shadingPattern)
			format.Stroke.IsApplied = True

			' Add a text group inside another group because it is recommended to change the Transform only for a single element in a group.
			Dim textGroup = page.Content.Elements.AddGroup().Elements.AddGroup()
			textGroup.Transform = PdfMatrix.CreateTranslation(25, 550)
			Using formattedText = New PdfFormattedText()
				formattedText.Font = New PdfFont("Helvetica", 96)
				formattedText.AppendLine("Hello ").Append("world!")

				' Draw the formatted text in the bottom-left corner of the group.
				textGroup.DrawText(formattedText, New PdfPoint(0, 0))
			End Using
			format = textGroup.Format
			' Don't fill the text, but make it a clipping region for next content - shading.
			format.Fill.IsApplied = False
			format.Clip.IsApplied = True
			' Add a bounding rectangle before (because it would not be visible otherwise because all following content is clipped by the text) text elements.
			Dim path = textGroup.Elements.AddPath(textGroup.Elements.First)
			path.AddRectangle(0, 0, 250, 250)
			path.Format.Stroke.IsApplied = True
			' Add shading content that is clipped by the text content.
			' In this case shading doesn't start from the bottom-left corner of the page, but from the bottom-left corner of the group.
			textGroup.Elements.AddShading(shading)

			' Add a path group inside another group because it is recommended to change the Transform only for a single element in a group.
			Dim pathGroup = page.Content.Elements.AddGroup().Elements.AddGroup()
			pathGroup.Transform = PdfMatrix.CreateTranslation(325, 550)
			path = pathGroup.Elements.AddPath()
			path.AddRectangle(0, 0, 250, 250)
			' Make path a clipping region for next content - shading.
			path.Format.Clip.IsApplied = True
			' Add shading content that is clipped by the path content.
			' In this case shading doesn't start from the bottom-left corner of the page, but from the bottom-left corner of the group.
			Dim shadingContent = pathGroup.Elements.AddShading(shading)
			' Make the shading 50% opaque.
			shadingContent.Format.Fill.Opacity = 0.5

			document.Save("Shadings.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Shadings.pdf") With {.UseShellExecute = True})
	End Sub
End Class
