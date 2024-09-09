Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Annotations
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' How to add a go-to action in a PDF document.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/add-go-to-action-in-pdf-document.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			GoToAction()
		End Sub

		Private Shared Sub GoToAction()
			Dim outFile As String = "GoToAction.pdf"
			Using pdf = New PdfDocument()
				' Add a new page.
				Dim firstPage = pdf.Pages.Add()
				' Add Link annotation.
				Dim annotation = firstPage.Annotations.AddLink(10, 700, 300, 100)
				annotation.Appearance.BorderColor = PdfColor.FromRgb(0, 255 \ 255, 0)

				annotation.Appearance.BorderDashPattern = PdfLineDashPatterns.Solid
				annotation.Appearance.BorderWidth = 3
				annotation.Appearance.BorderStyle = PdfBorderStyle.Solid

				Dim text = New PdfFormattedText() With {.FontSize = 16.0}
				text.Append("Click to go to the second page!")
				firstPage.Content.DrawText(text, New PdfPoint(50, 750))

				' Add a new page.
				Dim secondPage = pdf.Pages.Add()
				text = New PdfFormattedText() With {
					.FontSize = 72.0,
					.Color = PdfColors.Gray
				}
				text.Append("Page 2")
				secondPage.Content.DrawText(text, New PdfPoint(150, 400))
				' Add an action to go to the second page in the Link annotation.
				Dim action = annotation.Actions.AddGoToPageView(secondPage, PdfDestinationViewType.FitPage)
				' Save PDF Document.
				pdf.Save(outFile)
			End Using
			' Show the result.
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(outFile) With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
