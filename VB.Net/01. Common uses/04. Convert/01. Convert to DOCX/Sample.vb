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
		''' Convert PDF to DOCX.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-pdf-to-docx.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free license:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			' Load a PDF document.
			Using document = PdfDocument.Load(Path.GetFullPath("..\..\..\simple text.pdf"))
				' Save a PDF document to a DOCX file.
				document.Save("Output.docx")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Output.docx") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
