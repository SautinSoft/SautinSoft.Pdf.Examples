Imports System
Imports System.IO
Imports System.IO.Compression
Imports System.Collections.Generic
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports System.Linq
Imports SautinSoft.Pdf.Facades

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Split PDF files in C# and .NET.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/split-pdf-files.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free license:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			' Split PDF document by pages.
			' The each page will be saves as a separate PDF file: "Page 0.pdf", "Page 1.pdf" ...
			' Can work with relative and absolute paths.
			PdfSplitter.Split("..\..\..\005.pdf", PdfLoadOptions.Default, 0, Integer.MaxValue, Function(pageInd) $"Page {pageInd}.pdf")

			' The last parameter is "Func" to generate the output file name.
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Page 0.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
