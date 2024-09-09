Option Infer On

Imports System
Imports System.IO
Imports System.Reflection.Metadata
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Convert PDF to Jpeg.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-pdf-to-images.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free license:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			' Load a PDF document.
			Using document = PdfDocument.Load(Path.GetFullPath("..\..\..\simple text.pdf"))
				' Create image save options.
				Dim imageOptions = New ImageSaveOptions(ImageSaveFormat.Jpeg) With {
					.PageIndex = 0,
					.Width = 1240
				}

				' Save a PDF document to a JPEG file.
				document.Save("Output.jpg", imageOptions)
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Output.jpg") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
