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
		''' Convert scanned PDF with other language to Word
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-scanned-pdf-with-other-language-to-word.php
		''' </remarks>
		Shared Sub Main()
			Try
				'Before executing, download the required language file from the link: https://github.com/tesseract-ocr/tessdata/tree/main
				'Place the file in a folder convenient for you and specify the path to it.

				Dim tesseractLanguages As String = "ron"
				Dim tesseractData As String = Path.GetFullPath("..\..\..\tessdata")
				Dim tempFile As String = Path.Combine(tesseractData, Path.GetRandomFileName())
				Dim pdfDocument As PdfDocument = PdfDocument.Load("..\..\..\ARGW64125SX.pdf")
				Dim pdfDocument1 As New PdfDocument()
				Dim text As New PdfFormattedText()

				Using engine As New Tesseract.TesseractEngine(tesseractData, tesseractLanguages, Tesseract.EngineMode.Default)
					Dim collection = pdfDocument.Pages(0).Content.Elements.All().OfType(Of PdfImageContent)().ToList()
					Dim pdfPage1 = pdfDocument1.Pages.Add()
					pdfPage1.SetMediaBox(pdfDocument.Pages(0).MediaBox.Left, pdfDocument.Pages(0).MediaBox.Bottom, pdfDocument.Pages(0).MediaBox.Right, pdfDocument.Pages(0).MediaBox.Top)
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
													text.Append(iter.GetText(PageIteratorLevel.TextLine))
													pdfPage1.Content.DrawText(text, New PdfPoint(collection(i).Bounds.Left + liRect.X1 * scale, collection(i).Bounds.Top - liRect.Y1 * scale - text.Height))
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
				End Using
				pdfDocument1.Save("text.docx")
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
