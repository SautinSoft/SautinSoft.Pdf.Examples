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
		''' Convert scanned PDF to Word
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-scanned-pdf-to-word.php
		''' </remarks>
		Shared Sub Main()
			Try
				Dim tesseractLanguages As String = "eng"
				Dim tesseractData As String = Path.GetFullPath(".\tessdata")
				Dim tempFile As String = Path.Combine(tesseractData, Path.GetRandomFileName())
				Dim pdfDocument As PdfDocument = PdfDocument.Load("..\..\..\Scanned PDF.pdf")
				Dim text As New PdfFormattedText()

				Using engine As New Tesseract.TesseractEngine(tesseractData, tesseractLanguages, Tesseract.EngineMode.Default)
					For Each pdfPage In pdfDocument.Pages
						Dim collection = pdfPage.Content.Elements.All().OfType(Of PdfImageContent)().ToList()
						For i As Integer = 0 To collection.Count() - 1
							engine.DefaultPageSegMode = Tesseract.PageSegMode.Auto
							Using ms As New MemoryStream()
								collection(i).Save(ms, New ImageSaveOptions())

								Dim imgBytes() As Byte = ms.ToArray()
								Using img As Tesseract.Pix = Tesseract.Pix.LoadFromMemory(imgBytes)
									Using page = engine.Process(img, "Serachablepdf")
										Dim st = page.GetText()
										Dim scale As Double = Math.Min(collection(i).Bounds.Width / page.RegionOfInterest.Width, collection(i).Bounds.Height / page.RegionOfInterest.Height)

										Using iter = page.GetIterator()
											iter.Begin()

											Do
												Do
													Do
														Dim liRect As Rect
														iter.TryGetBoundingBox(PageIteratorLevel.TextLine, liRect)
														text.FontSize = liRect.Height * scale
														'text.Opacity = 0;
														text.Append(iter.GetText(PageIteratorLevel.TextLine))
														pdfPage.Content.DrawText(text, New PdfPoint(collection(i).Bounds.Left + liRect.X1 * scale, collection(i).Bounds.Top - liRect.Y1 * scale - text.Height))
														text.Clear()
													Loop While iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine)
												Loop While iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para)
											Loop While iter.Next(PageIteratorLevel.Block)
										End Using
									End Using
								End Using
							End Using
							collection(i).Collection.Remove(collection(i))
						Next i
					Next pdfPage
				End Using
				pdfDocument.Save("Result.docx")
				System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Result.docx") With {.UseShellExecute = True})
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
