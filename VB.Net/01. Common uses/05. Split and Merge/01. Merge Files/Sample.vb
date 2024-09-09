Option Infer On

Imports System
Imports System.IO
Imports System.Collections.Generic
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Facades

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Merge PDF files.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/merge-pdf-files.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free license:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			MergePdfFiles()
		End Sub

		Private Shared Sub MergePdfFiles()
			Dim inpFiles = New List(Of String)(Directory.GetFiles(Path.GetFullPath("..\..\..\"), "*.pdf"))
			Dim outFile As String = Path.GetFullPath("Merged.pdf")

			' Create a PDF merger.
			Dim merger = New PdfMerger()

			' Merge multiple PDF documents to the one.
			For Each inpFile In inpFiles
				merger.Append(inpFile)
			Next inpFile

			merger.Save(outFile)

			' Show the result.
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(outFile) With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
