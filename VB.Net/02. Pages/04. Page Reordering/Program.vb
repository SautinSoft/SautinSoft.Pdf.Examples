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
		''' PDF Reordering (Moving pages).
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/page-reordering.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			' Load a PDF document.
			Using document = PdfDocument.Load(Path.GetFullPath("..\..\..\simple text.pdf"))
				' Moving the first page to the third
				Dim page = document.Pages(0)
				document.Pages.Remove(page)
				document.Pages.InsertClone(2, page)

				' Save a PDF document.
				document.Save("Output.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Output.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
