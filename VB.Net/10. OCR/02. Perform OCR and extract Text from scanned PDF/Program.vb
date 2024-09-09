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
		''' Perform OCR and extract Text from scanned PDF
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/perform-ocr-and-extract-text-from-scanned-pdf.php
		''' </remarks>
		Shared Sub Main()
			Try
				Dim tesseractLanguages As String = "eng"
				Dim tesseractData As String = Path.GetFullPath(".\tessdata")
				Dim tempFile As String = Path.Combine(tesseractData, Path.GetRandomFileName())
				Dim pdfDocument As PdfDocument = PdfDocument.Load("..\..\..\Scanned.pdf")

				Using engine As New Tesseract.TesseractEngine(tesseractData, tesseractLanguages, Tesseract.EngineMode.Default)
					Dim k As Integer = 1
					For Each pdfPage In pdfDocument.Pages
' INSTANT VB WARNING: An assignment within expression was extracted from the following statement:
' ORIGINAL LINE: Console.WriteLine("<Page " + (k++) + ">");
						Console.WriteLine("<Page " & (k) & ">")
						k += 1
						Dim collection = pdfPage.Content.Elements.All().OfType(Of PdfImageContent)().ToList()
						For i As Integer = 0 To collection.Count() - 1
							engine.DefaultPageSegMode = Tesseract.PageSegMode.Auto
							Using ms As New MemoryStream()
								collection(i).Save(ms, New ImageSaveOptions())

								Dim imgBytes() As Byte = ms.ToArray()
								Using img As Tesseract.Pix = Tesseract.Pix.LoadFromMemory(imgBytes)
									Using page = engine.Process(img, "Serachablepdf")
										Dim st = page.GetText()
										Dim scale As Double = Math.Min(collection(i).Bounds.Width \ page.RegionOfInterest.Width, collection(i).Bounds.Height \ page.RegionOfInterest.Height)

										Using iter = page.GetIterator()
											iter.Begin()

											Do
												Do
													Do
														Do
															Console.Write(iter.GetText(PageIteratorLevel.Word))
															Console.Write(" "c)

															If iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word) Then
																Console.WriteLine()
															End If
														Loop While iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word)
													Loop While iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine)
												Loop While iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para)
											Loop While iter.Next(PageIteratorLevel.Block)
										End Using
									End Using
								End Using
							End Using
						Next i
					Next pdfPage
				End Using
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
