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
		''' Convert PDF to PDF/A using C# and .NET.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-pdf-to-pdfa-using-csharp-and-dotnet.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free license:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			' Load a PDF document.
			Using document = PdfDocument.Load(Path.GetFullPath("..\..\..\simple text.pdf"))
				' Create PDF save options.
				Dim pdfOptions = New PdfSaveOptions() With {.Version = PdfVersion.PDF_A_4E}

				' Save a PDF document.
				document.Save("Convert to PDF-A.pdf", pdfOptions)
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Convert to PDF-A.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
