Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
    Class Sample
        ''' <summary>
        ''' Convert PDF to PDF-A ZUGFeRD using VB.NET and .NET.
        ''' </summary>
        ''' <remarks>
        ''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-pdf-to-pdfa-zugferd.php
        ''' </remarks>
        Shared Sub Main(args As String())
            ' Before starting this example, please get a free trial key:
            ' https://sautinsoft.com/start-for-free/

            ' Apply the key here:
            ' PdfDocument.SetLicense("...")
            Dim inpFile As String = "..\..\..\ZUGFeRD\ZUGFeRD.pdf"
            Dim outFile As String = "..\..\..\ZUGFeRD\ZUGFeRD_Result.pdf"
            Dim xmlInfo As String = "..\..\..\ZUGFeRD\ZUGFeRD.xml"
            ' Load a PDF document.
            Using document = PdfDocument.Load(Path.GetFullPath(inpFile))
                ' Create PDF save options.
                Dim pdfOptions As New PdfSaveOptions() With {
                .Version = PdfVersion.PDF_A_3A,
                .FacturXXml = File.ReadAllText(xmlInfo)
                }

                ' Save a PDF document like the FacturX Zugferd.
                ' Read more information about Factur-X: https://fnfe-mpe.org/factur-x/

                document.Save(outFile, pdfOptions)
            End Using
        End Sub
    End Class
End Namespace