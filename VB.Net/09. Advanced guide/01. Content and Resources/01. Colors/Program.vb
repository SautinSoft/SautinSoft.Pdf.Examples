Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports System.Security.Cryptography
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Objects
Imports SautinSoft.Pdf.Content.Colors
Imports SautinSoft.Pdf.Text

Friend Class Program
	''' <summary>
	''' Work with Color
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/pdf-content-formatting-color.php
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
				formattedText.Font = New PdfFont("Helvetica", 24)

				' Three different ways to specify gray color in the DeviceGray color space:
				formattedText.Color = PdfColors.Gray
				formattedText.Append("Hello world! ")
				formattedText.Color = PdfColor.FromGray(0.5)
				formattedText.Append("Hello world! ")
				formattedText.Color = New PdfColor(PdfColorSpace.DeviceGray, 0.5)
				formattedText.AppendLine("Hello world!")

				' Three different ways to specify red color in the DeviceRGB color space:
				formattedText.Color = PdfColors.Red
				formattedText.Append("Hello world! ")
				formattedText.Color = PdfColor.FromRgb(1, 0, 0)
				formattedText.Append("Hello world! ")
				formattedText.Color = New PdfColor(PdfColorSpace.DeviceRGB, 1, 0, 0)
				formattedText.AppendLine("Hello world!")

				' Three different ways to specify yellow color in the DeviceCMYK color space:
				formattedText.Color = PdfColors.Yellow
				formattedText.Append("Hello world! ")
				formattedText.Color = PdfColor.FromCmyk(0, 0, 1, 0)
				formattedText.Append("Hello world! ")
				formattedText.Color = New PdfColor(PdfColorSpace.DeviceCMYK, 0, 0, 1, 0)
				formattedText.Append("Hello world!")

				page.Content.DrawText(formattedText, New PdfPoint(100, 500))
			End Using

			' Create an Indexed color space
			' as specified in Adobe
			' Base color space is DeviceRGB and the created Indexed color space consists of two colors:
			' at index 0: green color (0x00FF00)
			' at index 1: blue color (0x0000FF)
			Dim indexedColorSpaceArray = PdfArray.Create(4)
			indexedColorSpaceArray.Add(PdfName.Create("Indexed"))
			indexedColorSpaceArray.Add(PdfName.Create("DeviceRGB"))
			indexedColorSpaceArray.Add(PdfInteger.Create(1))
			indexedColorSpaceArray.Add(PdfString.Create(ChrW(&H00).ToString() & ChrW(&HFF).ToString() & ChrW(&H00).ToString() & ChrW(&H00).ToString() & ChrW(&H00).ToString() & ChrW(&HFF).ToString(), PdfEncoding.Byte, PdfStringForm.Hexadecimal))
			Dim indexedColorSpace = PdfColorSpace.FromArray(indexedColorSpaceArray)

			' Add a rectangle.
			' Fill it with color at index 0 (green) of the Indexed color space.
			' Stroke it with color at index 1 (blue) of the Indexed color space.
			Dim path = page.Content.Elements.AddPath()
			path.AddRectangle(100, 300, 200, 100)
			Dim format = path.Format
			format.Fill.IsApplied = True
			format.Fill.Color = New PdfColor(indexedColorSpace, 0)
			format.Stroke.IsApplied = True
			format.Stroke.Color = New PdfColor(indexedColorSpace, 1)
			format.Stroke.Width = 5

			document.Save("Colors.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Colors.pdf") With {.UseShellExecute = True})
	End Sub
End Class
