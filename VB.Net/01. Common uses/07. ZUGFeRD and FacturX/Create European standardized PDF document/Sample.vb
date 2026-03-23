Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Content
Imports System.Drawing

Namespace Sample
    Class Program
        ''' <remarks>
        ''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/create-european-standardized-pdf-document.php
        ''' </remarks>
        Shared Sub Main()
            ' Before starting this example, please get a free trial key:
            ' https://sautinsoft.com/start-for-free/

            ' Apply the key here:
            ' PdfDocument.SetLicense("...")
            Dim xmlInfo As String = "..\..\..\Facture.xml"

            Using document As New PdfDocument()

                Dim page = document.Pages.Add()

                Dim formattedText1 As New PdfFormattedText()
                Dim text1 As String = "Hello World"
                formattedText1.FontSize = 15
                formattedText1.FontFamily = New PdfFontFamily("Calibri")
                formattedText1.Append(text1)
                page.Content.DrawText(formattedText1, New PdfPoint(110, 650))

                Dim formattedText2 As New PdfFormattedText()
                Dim text2 As String = "This message was"
                formattedText2.FontSize = 16
                formattedText2.FontFamily = New PdfFontFamily("Times New Roman")
                formattedText2.FontStyle = PdfFontStyle.Italic
                formattedText2.Color = PdfColor.FromRgb(1, 0, 0)
                formattedText2.Append(text2)
                page.Content.DrawText(formattedText2, New PdfPoint(115, 632))

                Dim formattedText3 As New PdfFormattedText()
                Dim text3 As String = "created by SautinSoft"
                formattedText3.FontSize = 22
                formattedText3.FontStyle = PdfFontStyle.Italic
                formattedText3.FontFamily = New PdfFontFamily("Times New Roman")
                formattedText3.Color = PdfColor.FromRgb(1, 0, 0)
                formattedText3.Append(text3)
                page.Content.DrawText(formattedText3, New PdfPoint(110, 610))

                Dim formattedText4 As New PdfFormattedText()
                Dim text4 As String = "component!"
                formattedText4.FontSize = 22
                formattedText4.FontFamily = New PdfFontFamily("Times New Roman")
                formattedText4.Append(text4)
                page.Content.DrawText(formattedText4, New PdfPoint(303, 610))

                Dim pdfOptions As New PdfSaveOptions() With {
                    .Version = PdfVersion.PDF_A_3A,
                    .FacturXXml = File.ReadAllText(xmlInfo)
                }

                ' Save a PDF document like the FacturX Zugferd.
                ' Read more information about Factur-X: https://fnfe-mpe.org/factur-x/

                document.Save("Output.pdf", pdfOptions)
            End Using

        End Sub
    End Class
End Namespace