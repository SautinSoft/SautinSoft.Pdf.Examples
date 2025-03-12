Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Add hyperlinks to PDF files.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/hyperlinks.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")

			Using document = New PdfDocument()
				Dim page = document.Pages.Add()

				Dim secondPage = document.Pages.Add()

				Dim pageWidth = page.Size.Width

				Using formattedText = New PdfFormattedText()
					formattedText.FontSize = 24
					formattedText.Append("First page")
					Dim y As Double = 700
					Dim origin = New PdfPoint((pageWidth - formattedText.Width) \ 2, y)
					page.Content.DrawText(formattedText, origin)

					Dim image = PdfImage.Load("..\..\..\SautinSoft.png")
					y -= image.Size.Height + 100
					origin = New PdfPoint((pageWidth - image.Size.Width) \ 2, y)
					page.Content.DrawImage(image, origin)

					' Add a link annotation over the drawn image that opens a website.
					Dim link = page.Annotations.AddLink(origin.X, origin.Y, image.Size.Width, image.Size.Height)
					link.Actions.AddOpenWebLink("https://sautinsoft.com/")

					formattedText.Clear()
					formattedText.Append("Open file")
					y -= formattedText.Height + 100
					origin = New PdfPoint((pageWidth - formattedText.Width) \ 2, y)
					page.Content.DrawText(formattedText, origin)

					' Add a link annotation over the drawn text that opens a file.
					link = page.Annotations.AddLink(origin.X, origin.Y, formattedText.Width, formattedText.Height)
					link.Actions.AddOpenFile(pdfFile)

					formattedText.Clear()
					formattedText.Append("Go to second page")
					y -= formattedText.Height + 100
					origin = New PdfPoint((pageWidth - formattedText.Width) \ 2, y)
					page.Content.DrawText(formattedText, origin)

					' Add a link annotation over the drawn text that goes to the second page.
					link = page.Annotations.AddLink(origin.X, origin.Y, formattedText.Width, formattedText.Height)
					link.Actions.AddGoToPageView(secondPage, PdfDestinationViewType.FitPage)

					formattedText.Clear()
					formattedText.Append("Second page")
					origin = New PdfPoint((pageWidth - formattedText.Width) \ 2, 700)
					secondPage.Content.DrawText(formattedText, origin)
				End Using

				document.Save("Hyperlinks.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Hyperlinks.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
