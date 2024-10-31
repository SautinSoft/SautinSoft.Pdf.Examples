Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf

Module Sample
    ''' <remarks>
    ''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/save-pdf.php
    ''' </remarks>
    Sub Main(args As String())
        ' Before starting this example, please get a free 100-day trial key:
        ' https://sautinsoft.com/start-for-free/

        ' Apply the key here:
        ' PdfDocument.SetLicense("...")

        ' Load a PDF document.
        Using document As PdfDocument = PdfDocument.Load(Path.GetFullPath("..\..\..\simple text.pdf"))
            ' Save a PDF document.
            document.Save("Output.pdf")
        End Using

        System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Output.pdf") With {.UseShellExecute = True})
    End Sub
End Module

