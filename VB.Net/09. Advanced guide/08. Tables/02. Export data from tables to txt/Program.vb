Option Infer On

Imports System
Imports System.Globalization
Imports System.IO
Imports System.Reflection.Metadata
Imports System.Text.Json
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Find Tables
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/export-data-from-table-to-txt.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\tables.pdf")
			Dim writer = New StringWriter(CultureInfo.InvariantCulture)

			Using document = PdfDocument.Load(pdfFile)
				' Find Tables.
				Dim tables = document.Pages(0).Content.FindTables()

				Dim format As String = "{0,-20}|{1,-20}", separator As New String("-"c, 40)

				' Get text from tables.
				For Each table In tables
					For Each row In table.Rows
						writer.WriteLine(format, row.Cells(0).ToString(), row.Cells(1).ToString())
						writer.WriteLine(separator)
					Next row
					writer.WriteLine()
				Next table
			End Using

			Dim file = New FileStream("Output.txt", FileMode.Create)
			Dim streamWriter As New StreamWriter(file)
			streamWriter.WriteLine(writer.ToString())
			streamWriter.Close()
			file.Close()

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Output.txt") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
