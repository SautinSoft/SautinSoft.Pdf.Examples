Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports System
Imports Tesseract
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq

Namespace OCR
    Friend Class OCR
        ''' <summary>
        ''' OCR a PDF document containing text as vector graphics
        ''' </summary>
        ''' <remarks>
        ''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/ocr-a-pdf-document-containing-text-as-vector-graphics.php
        ''' </remarks>
        Public Shared Sub Main()
            Dim liRect As Rect = Nothing
            Try
                Dim tesseractLanguages = "eng"
                Dim tesseractData = Path.GetFullPath(".\tessdata")
                Dim tempFile As String = Path.Combine(tesseractData, Path.GetRandomFileName())
                Dim pdfDocument As PdfDocument = PdfDocument.Load("..\..\..\Vectorized text.pdf")
                Dim mss As List(Of MemoryStream) = New List(Of MemoryStream)()
                Dim ms As MemoryStream = New MemoryStream()
                pdfDocument.Save(ms, New ImageSaveOptions())
                pdfDocument = New PdfDocument()
                Dim text As PdfFormattedText = New PdfFormattedText()

                Using engine As TesseractEngine = New TesseractEngine(tesseractData, tesseractLanguages, EngineMode.Default)
                    Dim pdfPage = pdfDocument.Pages.Add()
                    engine.DefaultPageSegMode = PageSegMode.Auto
                    If True Then
                        Dim imgBytes As Byte() = ms.ToArray()
                        Using img = Pix.LoadFromMemory(imgBytes)
                            Using page = engine.Process(img, "Serachablepdf")
                                Dim st = page.GetText()
                                Dim scale = Math.Min(pdfPage.Size.Width / page.RegionOfInterest.Width, pdfPage.Size.Height / page.RegionOfInterest.Height)

                                Using iter = page.GetIterator()
                                    iter.Begin()

                                    Do
                                        Do
                                            Do
                                                iter.TryGetBoundingBox(PageIteratorLevel.TextLine, liRect)
                                                text.FontSize = liRect.Height * scale
                                                'text.Opacity = 0;
                                                text.Append(iter.GetText(PageIteratorLevel.TextLine))
                                                pdfPage.Content.DrawText(text, New PdfPoint(liRect.X1 * scale, pdfPage.Size.Height - liRect.Y1 * scale - text.Height))
                                                text.Clear()
                                            Loop While iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine)
                                        Loop While iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para)
                                    Loop While iter.Next(PageIteratorLevel.Block)
                                End Using
                            End Using
                        End Using
                    End If
                End Using
                pdfDocument.Save("text.docx")
                Process.Start(New ProcessStartInfo("text.docx") With {
                    .UseShellExecute = True
                })
            Catch e As Exception
                Console.WriteLine()
                Console.WriteLine("Please be sure that you have Language data files (*.traineddata) in your folder ""tessdata""")
                Console.WriteLine("The Language data files can be download from here: https://github.com/tesseract-ocr/tessdata_fast")
                Console.ReadKey()
                Throw New Exception("Error Tesseract: " & e.Message)
            Finally

            End Try
        End Sub
    End Class
End Namespace
