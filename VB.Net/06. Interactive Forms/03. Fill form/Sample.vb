Option Infer On

Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Fill in PDF interactive forms.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/fill-in-pdf-interactive-forms.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\Form.pdf")

			'Load PDF Document with forms.
			Using document = PdfDocument.Load(pdfFile)
				' Fill the form fields.
				document.Form.Fields("FullName").Value = "Jane Doe"
				document.Form.Fields("ID").Value = "0123456789"
				document.Form.Fields("Gender").Value = "Female"
				document.Form.Fields("Married").Value = "Yes"
				document.Form.Fields("City").Value = "Berlin"
				document.Form.Fields("Language").Value = New String() { "German", "Italian" }
				document.Form.Fields("Notes").Value = "Notes first line" & vbCr & "Notes second line" & vbCr & "Notes third line"

				' Save PDF Document.
				document.Save("FormFilled.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("FormFilled.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
