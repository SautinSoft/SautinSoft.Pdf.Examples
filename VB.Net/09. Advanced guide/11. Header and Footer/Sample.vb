Option Infer On

Imports System
Imports System.Globalization
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Header and Footer.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/header-footer.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")

			Using document = PdfDocument.Load(pdfFile)
				Dim marginLeft As Double = 20, marginTop As Double = 10, marginRight As Double = 20, marginBottom As Double = 10

				Using formattedText = New PdfFormattedText()
					formattedText.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture))

					' Add a header with the current date and time to all pages.
					For Each page In document.Pages
						' Set the location of the bottom-left corner of the text.
						' We want the top-left corner of the text to be at location (marginLeft, marginTop)
						' from the top-left corner of the page.
						' NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
						' and the positive y axis extends vertically upward.
						Dim x As Double = marginLeft, y As Double = page.CropBox.Top - marginTop - formattedText.Height

						page.Content.DrawText(formattedText, New PdfPoint(x, y))
					Next page

					' Add a footer with the current page number to all pages.
					Dim pageCount As Integer = document.Pages.Count, pageNumber As Integer = 0
					For Each page In document.Pages
						pageNumber += 1

						formattedText.Clear()
						formattedText.Append(String.Format("Page {0} of {1}", pageNumber, pageCount))

						' Set the location of the bottom-left corner of the text.
						Dim x As Double = page.CropBox.Width - marginRight - formattedText.Width, y As Double = marginBottom

						page.Content.DrawText(formattedText, New PdfPoint(x, y))
					Next page
				End Using

				document.Save("Header and Footer.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Header and Footer.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
