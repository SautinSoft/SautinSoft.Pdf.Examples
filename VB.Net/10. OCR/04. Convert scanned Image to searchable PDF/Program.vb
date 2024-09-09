Option Infer On

Imports Net.Pkcs11Interop.HighLevelAPI.MechanismParams
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
		''' Convert scanned Image to searchable PDF
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-scanned-image-to-searchable-pdf.php
		''' </remarks>
		Shared Sub Main()
			Try
				Dim tesseractLanguages As String = "eng"
				Dim tesseractData As String = Path.GetFullPath(".\tessdata")
				Dim tempFile As String = Path.Combine(tesseractData, Path.GetRandomFileName())
				Dim pdfDocument As New PdfDocument()
				Dim pdfPage = pdfDocument.Pages.Add()
				Dim text As New PdfFormattedText()

				Using engine As New Tesseract.TesseractEngine(tesseractData, tesseractLanguages, Tesseract.EngineMode.Default)

					engine.DefaultPageSegMode = Tesseract.PageSegMode.Auto
					Using ms As New MemoryStream()
						Call (New FileStream("..\..\..\Potato Beetle.png", FileMode.Open)).CopyTo(ms)

						Dim imgBytes() As Byte = ms.ToArray()
						Using img As Tesseract.Pix = Tesseract.Pix.LoadFromMemory(imgBytes)
							Using page = engine.Process(img, "Serachablepdf")
								Dim st = page.GetText()
								Dim scale As Double = Math.Min(pdfPage.Size.Width / page.RegionOfInterest.Width, pdfPage.Size.Height / page.RegionOfInterest.Height)

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
													pdfPage.Content.DrawText(text, New PdfPoint(liRect.X1 * scale, pdfPage.Size.Height - liRect.Y1 * scale - text.Height))
													text.Clear()
											Loop While iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine)
										Loop While iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para)
									Loop While iter.Next(PageIteratorLevel.Block)
								End Using
							End Using
						End Using
					End Using
				End Using
				pdfDocument.Save("Result.pdf")
				System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Result.pdf") With {.UseShellExecute = True})
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
