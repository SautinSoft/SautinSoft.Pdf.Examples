Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Content

Friend Class Program
	''' <summary>
	''' How to set text alignment and positioning.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/text-alignment-and-positioning.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = New PdfDocument()
			' Add a new page.
			Dim page = document.Pages.Add()
			Dim margin As Double = 10
			Using formattedText = New PdfFormattedText()
				' Set up and fill the PdfFormattedText object.
				formattedText.TextAlignment = PdfTextAlignment.Left
				formattedText.MaxTextWidth = 100
				formattedText.Append("This text is left aligned, ").Append("placed in the top-left corner of the page and ").Append("its width should not exceed 100 points.")
				' Draw left-aligned text in the top-left corner of the page.
				page.Content.DrawText(formattedText, New PdfPoint(margin, page.CropBox.Top - margin - formattedText.Height))
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.TextAlignment = PdfTextAlignment.Center
				formattedText.MaxTextWidth = 200
				formattedText.Append("This text is center aligned, ").Append("placed in the top-center part of the page ").Append("and its width should not exceed 200 points.")
				' Draw center-aligned text at the top-center part of the page.
' INSTANT VB WARNING: Instant VB cannot determine whether both operands of this division are integer types - if they are then you should use the VB integer division operator:
				page.Content.DrawText(formattedText, New PdfPoint((page.CropBox.Width - formattedText.MaxTextWidth) / 2, page.CropBox.Top - margin - formattedText.Height))
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.TextAlignment = PdfTextAlignment.Right
				formattedText.MaxTextWidth = 100
				formattedText.Append("This text is right aligned, ").Append("placed in the top-right corner of the page ").Append("and its width should not exceed 100 points.")
				' Draw right-aligned text in the top-right corner of the page.
				page.Content.DrawText(formattedText, New PdfPoint(page.CropBox.Width - margin - formattedText.MaxTextWidth, page.CropBox.Top - margin - formattedText.Height))
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.TextAlignment = PdfTextAlignment.Left
				formattedText.MaxTextWidth = 100
				formattedText.Append("This text is left aligned, ").Append("placed in the bottom-left corner of the page and ").Append("its width should not exceed 100 points.")
				' Draw left-aligned text in the bottom-left corner of the page.
				page.Content.DrawText(formattedText, New PdfPoint(margin, margin))
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.TextAlignment = PdfTextAlignment.Center
				formattedText.MaxTextWidth = 200
				formattedText.Append("This text is center aligned, ").Append("placed in the bottom-center part of the page and ").Append("its width should not exceed 200 points.")
				' Draw center-aligned text at the bottom-center part of the page.
' INSTANT VB WARNING: Instant VB cannot determine whether both operands of this division are integer types - if they are then you should use the VB integer division operator:
				page.Content.DrawText(formattedText, New PdfPoint((page.CropBox.Width - formattedText.MaxTextWidth) / 2, margin))
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.TextAlignment = PdfTextAlignment.Right
				formattedText.MaxTextWidth = 100
				formattedText.Append("This text is right aligned, ").Append("placed in the bottom-right corner of the page and ").Append("its width should not exceed 100 points.")
				' Draw right-aligned text in the bottom-right corner of the page.
				page.Content.DrawText(formattedText, New PdfPoint(page.CropBox.Width - margin - formattedText.MaxTextWidth, margin))
				' Clear the PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill the PdfFormattedText object.
				formattedText.TextAlignment = PdfTextAlignment.Justify
				formattedText.MaxTextWidths = New Double() { 200, 150, 100 }
				formattedText.Append("This text has justified alignment, ").Append("is placed in the center of the page and ").Append("its first line should not exceed 200 points, ").Append("its second line should not exceed 150 points and ").Append("its third and all other lines should not exceed 100 points.")
				' Draw justify-aligned text in the center part of the page.
				' Center the text based on the width of the most lines, which is formattedText.MaxTextWidths[2].
' INSTANT VB WARNING: Instant VB cannot determine whether both operands of this division are integer types - if they are then you should use the VB integer division operator:
				page.Content.DrawText(formattedText, New PdfPoint((page.CropBox.Width - formattedText.MaxTextWidths(2)) / 2, (page.CropBox.Height - formattedText.Height) \ 2))
				' Save a PDF Document.
				document.Save("Alignment and Positioning.pdf")
			End Using
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Alignment and Positioning.pdf") With {.UseShellExecute = True})
	End Sub
End Class
