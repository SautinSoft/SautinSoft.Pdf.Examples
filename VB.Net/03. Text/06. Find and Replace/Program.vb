Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Find and Replace text in PDF document using C# and .NET
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/find-and-replace-text-in-pdf-document-csharp-dotnet.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free license:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim inpFile As String = Path.GetFullPath("..\..\..\simple text.pdf")
			Dim outFile As String = Path.ChangeExtension(inpFile, ".res.pdf")

			Using document As PdfDocument = PdfDocument.Load(inpFile)
				' Assume we want to find the word "North"
				' and replace it to the "South".
				Dim textFrom As String = "North"
				Dim textTo As String = "South"

				' Iterate by all pages
				For Each page In document.Pages
					' Find the text.
					Dim texts = page.Content.GetText().Find(textFrom)

					' Get the text coordinates and font from 1st Element;
					' Draw the new text.
					For Each PDFtext In texts
						For Each el In PDFtext.Elements
							' Get the text formatting, coordinates;
							' Draw the new text "South".
							Using formattedText = New PdfFormattedText()
								formattedText.Font = el.Format.Text.Font
								' Set "orange" color
								formattedText.Color = PdfColor.FromRgb(1, 0.647, 0)
								formattedText.AppendLine(textTo)
								page.Content.DrawText(formattedText, New PdfPoint(PDFtext.Bounds.Left, PDFtext.Bounds.Bottom - PDFtext.Bounds.Height))
							End Using
							Exit For
						Next el
						' Remove the text.
						PDFtext.Redact()
					Next PDFtext
				Next page
				document.Save(outFile)
			End Using
			' Show the result.
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(outFile) With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
