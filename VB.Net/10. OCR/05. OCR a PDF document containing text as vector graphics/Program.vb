Option Infer On

Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Objects
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
		Shared Sub Main()
			Try
				Dim tesseractLanguages As String = "eng"
				Dim tesseractData As String = Path.GetFullPath(".\tessdata")
				Dim tempFile As String = Path.Combine(tesseractData, Path.GetRandomFileName())
				Dim pdfDocument As PdfDocument = PdfDocument.Load("..\..\..\Vectorized text.pdf")
				Dim mss As New List(Of MemoryStream)()
				For i As Integer = 0 To pdfDocument.Pages.Count - 1
					Dim ms As New MemoryStream()
					pdfDocument.Save(ms, New ImageSaveOptions() With {.PageIndex = i})
					mss.Add(ms)
				Next i
				pdfDocument = New PdfDocument()
				Dim text As New PdfFormattedText()

				Using engine As New Tesseract.TesseractEngine(tesseractData, tesseractLanguages, Tesseract.EngineMode.Default)
					For Each ms In mss
						Dim pdfPage = pdfDocument.Pages.Add()
' INSTANT VB TASK: Local functions are not converted by Instant VB:
'						engine.DefaultPageSegMode = Tesseract.PageSegMode.Auto
'						{
'							byte[] imgBytes = ms.ToArray();
'							using (Tesseract.Pix img = Tesseract.Pix.LoadFromMemory(imgBytes))
'							{
'								using (var page = engine.Process(img, "Serachablepdf"))
'								{
'									var st = page.GetText();
'									double scale = Math.Min(pdfPage.Size.Width / page.RegionOfInterest.Width, pdfPage.Size.Height / page.RegionOfInterest.Height);
'
'									using (var iter = page.GetIterator())
'									{
'										iter.Begin();
'
'										do
'										{
'											do
'											{
'												do
'												{
'													iter.TryGetBoundingBox(PageIteratorLevel.TextLine, out Rect liRect);
'													text.FontSize = liRect.Height * scale;
'													'text.Opacity = 0;
'													text.Append(iter.GetText(PageIteratorLevel.TextLine));
'													pdfPage.Content.DrawText(text, New PdfPoint(liRect.X1 * scale, pdfPage.Size.Height - liRect.Y1 * scale - text.Height));
'													text.Clear();
'												} while (iter.@Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
'											} while (iter.@Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
'										} while (iter.@Next(PageIteratorLevel.Block));
'									}
'								}
'							}
'						}
					Next ms
				End Using
				pdfDocument.Save("text.docx")
				System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("text.docx") With {.UseShellExecute = True})
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
