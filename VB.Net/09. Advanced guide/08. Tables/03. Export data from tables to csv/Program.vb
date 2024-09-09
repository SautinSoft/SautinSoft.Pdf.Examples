Option Infer On

Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Text.RegularExpressions
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		Shared Sub Main(ByVal args() As String)
			''' <summary>
			''' Find Tables.
			''' </summary>
			''' <remarks>
			''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/export-data-from-table-to-csv.php
			''' </remarks>
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\Item.pdf")
			Dim csv As String = ""

			Using document = PdfDocument.Load(pdfFile)
				' Find Tables.
				Dim tables = document.Pages(0).Content.FindTables()
				Dim col As Integer = -1
				Dim sum As Double = 0

				' Get text from tables to CSV string.
				For Each table In tables
					For Each row In table.Rows
						For i As Integer = 0 To row.Cells.Count - 1
							If col > -1 AndAlso i = col Then
								sum += Convert.ToDouble(row.Cells(i).ToString())
							End If
							If row.Cells(i).ToString().Contains("Total Price") Then
								col = i
							End If
							csv &= row.Cells(i).ToString() & ";"c
						Next i
						csv &= vbLf
					Next row
					csv &= "Total;;;" & sum.ToString()
					sum = 0
					col = -1
					csv &= vbLf
				Next table
			End Using

			Dim stream = New FileStream("Output.csv", FileMode.Create)
			stream.Close()
			File.WriteAllText("Output.csv", csv)

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Output.csv") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
