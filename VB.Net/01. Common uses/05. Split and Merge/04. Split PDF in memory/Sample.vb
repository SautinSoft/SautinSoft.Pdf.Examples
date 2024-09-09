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
		''' Split PDF documents in memory using C# and .NET.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/merge-pdf-documents-in-memory-using-csharp-and-dotnet.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free lisense:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			SplitPdfInMemory()
		End Sub
		Private Shared Sub SplitPdfInMemory()
			Dim page As Integer = 0
			Using fs = New FileStream("..\..\..\005.pdf", FileMode.Open, FileAccess.ReadWrite)
				For Each stream In PdfSplitter.Split(fs, PdfLoadOptions.Default, 0, Integer.MaxValue)
' INSTANT VB WARNING: An assignment within expression was extracted from the following statement:
' ORIGINAL LINE: using var output = new FileStream(string.Format("Page {0}.pdf", ++page), FileMode.Create, FileAccess.ReadWrite);
					Using page += 1
Dim output = New FileStream($"Page {page}.pdf", FileMode.Create, FileAccess.ReadWrite)
						stream.CopyTo(output)
					End Using
				Next stream
	
				' Show the "Page 5.pdf"
				System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Page 5.pdf") With {.UseShellExecute = True})
			End Using
		End Sub
	End Class
End Namespace
