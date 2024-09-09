Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Watermarks.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/watermarks.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")

			Using document = PdfDocument.Load(pdfFile)
				' Load the watermark from a file.
				Dim image = PdfImage.Load("..\..\..\WatermarkImage.png")

				For Each page In document.Pages
					' Make sure the watermark is correctly transformed even if
					' the page has a custom crop box origin, is rotated, or has custom units.
					Dim transform = page.Transform
					transform.Invert()

					' Center the watermark on the page.
					Dim pageSize = page.Size
					transform.Translate((pageSize.Width - 1) \ 2, (pageSize.Height - 1) \ 2)

					' Calculate the scaling factor so that the watermark fits the page.
					Dim cropBox = page.CropBox
					Dim scale = Math.Min(cropBox.Width, cropBox.Height)

					' Scale the watermark so that it fits the page.
					transform.Scale(scale, scale, 0.5, 0.5)

					' Draw the centered and scaled watermark.
					page.Content.DrawImage(image, transform)
				Next page

				document.Save("Watermark Images.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Watermark Images.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
