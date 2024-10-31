Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/create-pdf.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
            ' Before starting this example, please get a free 100-day trial key:
            ' https://sautinsoft.com/start-for-free/

            ' Apply the key here:
            ' PdfDocument.SetLicense("...");

            Using document = New PdfDocument()
                Dim page = document.Pages.Add()
                Dim formattedText1 = New PdfFormattedText()
                Dim text1 = "Hello World"
                formattedText1.FontSize = 15
                formattedText1.FontFamily = New PdfFontFamily("Calibri")
                formattedText1.Append(text1)
                page.Content.DrawText(formattedText1, New PdfPoint(110, 650))
                Dim formattedText2 = New PdfFormattedText()
                Dim text2 = "This message was"
                formattedText2.FontSize = 16
                formattedText2.FontFamily = New PdfFontFamily("Times New Roman")
                formattedText2.FontStyle = PdfFontStyle.Italic
                formattedText2.Color = PdfColor.FromRgb(1, 0, 0)
                formattedText2.Append(text2)
                page.Content.DrawText(formattedText2, New PdfPoint(115, 632))
                Dim formattedText3 = New PdfFormattedText()
                Dim text3 = "created by SautinSoft"
                formattedText3.FontSize = 22
                formattedText3.FontStyle = PdfFontStyle.Italic
                formattedText3.FontFamily = New PdfFontFamily("Times New Roman")
                formattedText3.Color = PdfColor.FromRgb(1, 0, 0)
                formattedText3.Append(text3)
                page.Content.DrawText(formattedText3, New PdfPoint(110, 610))
                Dim formattedText4 = New PdfFormattedText()
                Dim text4 = "component!"
                formattedText4.FontSize = 22
                formattedText4.FontFamily = New PdfFontFamily("Times New Roman")
                formattedText4.Append(text4)
                page.Content.DrawText(formattedText4, New PdfPoint(303, 610))
                document.Save("Output.pdf")
            End Using
        End Sub
	End Class
End Namespace
