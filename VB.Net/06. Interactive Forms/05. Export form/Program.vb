Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Export form fields data to fdf/xfdf/json document.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/export-interactive-forms.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\FormFilled.pdf")

			Using document = PdfDocument.Load(pdfFile)
				'Export form data as fdf stream.
				Dim fdfFile = New FileStream("fdfOut.fdf", FileMode.Create)
				document.Form.ExportData(fdfFile, SautinSoft.Pdf.Forms.PdfFormDataFormat.FDF)

				'Export form data to xfdf file.
				document.Form.ExportData("xfdfOut.xfdf")

				'Export form data to json file.
				document.Form.ExportData("jsonOut.json")
			End Using
		End Sub
	End Class
End Namespace
