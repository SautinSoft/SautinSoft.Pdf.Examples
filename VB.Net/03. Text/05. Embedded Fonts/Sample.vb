Option Infer On

Imports System
Imports System.IO
Imports System.Linq
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Using Embedded Fonts
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/embedded-fonts.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free license:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Using document = New PdfDocument()
				Dim page = document.Pages.Add()

				Using formattedText = New PdfFormattedText()
					formattedText.FontSize = 48
					formattedText.LineHeight = 72

					' Use the font family 'Almonte Snow' whose font file is located in the 'Resources' directory.
					formattedText.FontFamily = New PdfFontFamily("..\..\..\Resources", "Almonte Snow")
					formattedText.AppendLine("Hello World 1!")

					' Use the font family 'Almonte Woodgrain' whose font file is located in the 'Resources' location of the current assembly.
					formattedText.FontFamily = New PdfFontFamily("..\..\..\Resources", "Almonte Woodgrain")
					formattedText.AppendLine("Hello World 2!")

					' Another way to use the font family 'Almonte Snow' whose font file is located in the 'Resources' directory.
					formattedText.FontFamily = PdfFonts.GetFontFamilies("..\..\..\Resources").First(Function(ff) ff.Name = "Almonte Snow")
					formattedText.AppendLine("Hello World 3!")

					' Another way to use the font family 'Almonte Woodgrain' whose font file is located in the 'Resources' location of the current assembly.
					formattedText.FontFamily = PdfFonts.GetFontFamilies("..\..\..\Resources").First(Function(ff) ff.Name = "Almonte Woodgrain")
					formattedText.Append("Hello World 4!")

					' Draw this text.
					page.Content.DrawText(formattedText, New PdfPoint(100, 500))
				End Using
				' Save PDF Document.
				document.Save("Private Fonts.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Private Fonts.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
