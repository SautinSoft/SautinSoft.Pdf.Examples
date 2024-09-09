Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Add page numbers to a PDF document in C# and .NET
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/add-page-numbers-to-a-pdf-document-in-csharp-dotnet.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free license:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim inpFile As String = Path.GetFullPath("..\..\..\Wine turism.pdf")
			Dim outFile As String = Path.GetFullPath("Result.pdf")

			Using document As PdfDocument = PdfDocument.Load(inpFile)
				' Iterate by all pages
				Dim pageNum As Integer = 1
				For Each page In document.Pages
					' Draw the page numbering atop of all.
					' The method PdfContent(PdfContentGroup).DrawText always adds text to the end of the content.
					Using formattedText = New PdfFormattedText()
						formattedText.Font = New PdfFont(New PdfFontFace("Helvetica"), 16.0)
						' Set "orange" color
						formattedText.Color = PdfColor.FromRgb(1, 0.647, 0)
' INSTANT VB WARNING: An assignment within expression was extracted from the following statement:
' ORIGINAL LINE: formattedText.AppendLine(string.Format("Page {0}", pageNum++));
						formattedText.AppendLine($"Page {pageNum}")
						pageNum += 1
						page.Content.DrawText(formattedText, New PdfPoint((page.CropBox.Width \ 2) - formattedText.Width, page.CropBox.Height - 50))

						' Because of the trial version, we'll add page numbers only to two pages.
						If pageNum > 2 Then
							Exit For
						End If
					End Using
				Next page
				document.Save(outFile)
			End Using
			' Show the result.
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(outFile) With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
