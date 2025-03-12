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
		''' Merge PDF documents in memory using C# and .NET.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/merge-pdf-documents-in-memory-using-csharp-and-dotnet.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			MergePdfInMemory()
		End Sub
		Private Shared Sub MergePdfInMemory()
			' In this example we are using files only to get input data and show the result.
			Dim resultPath As String = "Result.pdf"
			' The whole merge process will be done completely in memory. 

			' The list with PDFs. The each document stored as bytes array.
			Dim pdfDocs As New List(Of Byte())()
			For Each f In Directory.GetFiles("..\..\..\", "*.pdf")
				pdfDocs.Add(File.ReadAllBytes(f))
			Next f

			' Create a PDF merger.
			Dim merger = New PdfMerger()

			' Iterate by documents and append them.
			For Each pdfDoc In pdfDocs
				Using ms = New MemoryStream(pdfDoc)
					merger.Append(ms)
				End Using
			Next pdfDoc

			' Save the merged PDF to a MemoryStream.
			Using msMerged = New MemoryStream()
				merger.Save(msMerged)
				' Save the result to a file to show.
				File.WriteAllBytes(resultPath, msMerged.ToArray())
			End Using

			' Show the result.
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(resultPath) With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
