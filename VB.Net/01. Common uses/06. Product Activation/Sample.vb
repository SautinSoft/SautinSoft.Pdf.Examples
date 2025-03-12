Option Infer On

Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports System
Imports System.Collections.Specialized.BitVector32

Namespace Sample
	Friend Class Sample
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			ProductActivation()
		End Sub

		''' <summary>
		''' PDF .Net activation.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/getting-started/product-activation.php
		''' </remarks>
		Private Shared Sub ProductActivation()
			' PDF .Net activation.

			' You will get own serial number after purchasing the license.
			' If you will have any questions, email us to sales@sautinsoft.com or ask at online chat https://www.sautinsoft.com.

			Dim serial As String = "1234567890"

			' NOTICE: Place this line firstly, before creating of the PdfDocument object.
			  PdfDocument.SetLicense(serial)

			Using document = New PdfDocument()
				Using formattedText = New PdfFormattedText()
					Dim page = document.Pages.Add()
					formattedText.Append("Hello from SautinSoft")
					page.Content.DrawText(formattedText, New PdfPoint(250, 330))
				End Using
				document.Save("Output.pdf")
				System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Output.pdf") With {.UseShellExecute = True})
			End Using
		End Sub
	End Class
End Namespace
