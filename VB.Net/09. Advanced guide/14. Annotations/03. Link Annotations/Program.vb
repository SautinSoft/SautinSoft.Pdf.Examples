Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Annotations

Friend Class Program
	''' <summary>
	''' Text Annotations.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/link-annotations.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = New PdfDocument()
			' Add a page.
			Dim page = document.Pages.Add()

			Using formattedText = New PdfFormattedText()
				' Set font family and size.
				' All text appended next uses the specified font family and size.
				formattedText.FontFamily = New PdfFontFamily("Calibri")
				formattedText.FontSize = 12

				formattedText.AppendLine("Hello World")

				' Reset font family and size for all text appended next.
				formattedText.FontFamily = New PdfFontFamily("Times New Roman")
				formattedText.FontSize = 14
				formattedText.FontStyle = PdfFontStyle.Italic
				formattedText.Color = PdfColor.FromRgb(1, 0, 0)
				formattedText.AppendLine(" This message was ")

				' Set font style and color for all text appended next.
				formattedText.FontFamily = New PdfFontFamily("Archi")
				formattedText.FontSize = 18

				formattedText.Append("created by SautinSoft")

				' Reset font style and color for all text appended next.
				formattedText.FontStyle = PdfFontStyle.Normal
				formattedText.Color = PdfColor.FromRgb(0, 0, 0)

				formattedText.Append(" component!")

				' Set the location of the bottom-left corner of the text.
				' We want top-left corner of the text to be at location (100, 100)
				' from the top-left corner of the page.
				' NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
				' and the positive y axis extends vertically upward.
				Dim x As Double = 100, y As Double = page.CropBox.Top - 100 - formattedText.Height

				' Draw text to the page.
				page.Content.DrawText(formattedText, New PdfPoint(x, y))

				'Find text "SautinSoft" and add Link annotations...
				Dim text = page.Content.GetText().Find("SautinSoft")
				For Each item In text
					Dim link = page.Annotations.AddLink(item.Bounds.Left, item.Bounds.Bottom, item.Bounds.Width, item.Bounds.Height)
					link.Actions.AddOpenWebLink("https://sautinsoft.com/")
				Next item
			End Using
			document.Save("Writing.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Writing.pdf") With {.UseShellExecute = True})
	End Sub
End Class
