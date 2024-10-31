Imports System
Imports System.IO
Imports System.Collections.Generic
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Facades

Module Sample
    ''' <summary>
    ''' Split PDF documents in memory using VB.NET.
    ''' </summary>
    ''' <remarks>
    ''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/merge-pdf-documents-in-memory-using-csharp-and-dotnet.php
    ''' </remarks>
    Sub Main(args As String())
        ' Before starting this example, please get a free 100-day trial key:
        ' https://sautinsoft.com/start-for-free/

        ' Apply the key here:
        ' PdfDocument.SetLicense("...")

        SplitPdfInMemory()
    End Sub

    Sub SplitPdfInMemory()
        Dim page As Integer = 0
        Using fs As New FileStream("..\..\..\005.pdf", FileMode.Open, FileAccess.ReadWrite)
            For Each stream In PdfSplitter.Split(fs, PdfLoadOptions.Default, 0, Integer.MaxValue)
                Using output As New FileStream($"Page {page + 1}.pdf", FileMode.Create, FileAccess.ReadWrite)
                    stream.CopyTo(output)
                End Using
                page += 1
            Next
        End Using

        ' Show the "Page 5.pdf"
        System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Page 5.pdf") With {.UseShellExecute = True})
    End Sub
End Module

