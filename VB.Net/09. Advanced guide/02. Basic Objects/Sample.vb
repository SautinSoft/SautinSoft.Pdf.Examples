Option Infer On

Imports System
Imports System.Globalization
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Objects
Imports SautinSoft.Pdf.Text

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Use basic PDF objects for currently unsupported PDF features.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/basic-objects.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")

			Using document = PdfDocument.Load(pdfFile)
				' Get document's trailer dictionary.
				Dim trailer = document.GetDictionary()
				' Get document catalog dictionary from the trailer.
				Dim catalog = CType(CType(trailer(PdfName.Create("Root")), PdfIndirectObject).Value, PdfDictionary)

				' Either retrieve "PieceInfo" entry value from document catalog or create a page-piece dictionary and set it to document catalog under "PieceInfo" entry.
				Dim pieceInfo As PdfDictionary
				Dim pieceInfoKey = PdfName.Create("PieceInfo")
				Dim pieceInfoValue = catalog(pieceInfoKey)
				Select Case pieceInfoValue.ObjectType
					Case PdfBasicObjectType.Dictionary
						pieceInfo = CType(pieceInfoValue, PdfDictionary)
					Case PdfBasicObjectType.IndirectObject
						pieceInfo = CType(CType(pieceInfoValue, PdfIndirectObject).Value, PdfDictionary)
					Case PdfBasicObjectType.Null
						pieceInfo = PdfDictionary.Create()
						catalog(pieceInfoKey) = PdfIndirectObject.Create(pieceInfo)
					Case Else
						Throw New InvalidOperationException("PieceInfo entry must be dictionary.")
				End Select

				' Create page-piece data dictionary for "SautinSoft.Pdf" conforming product and set it to page-piece dictionary.
				Dim data = PdfDictionary.Create()
				pieceInfo(PdfName.Create("SautinSoft.Pdf")) = data

				' Create a private data dictionary that will hold private data that "SautinSoft.Pdf" conforming product understands.
				Dim privateData = PdfDictionary.Create()
				data(PdfName.Create("Data")) = privateData

				' Set "Title" and "Version" entries to private data.
				privateData(PdfName.Create("Title")) = PdfString.Create("SautinSoft PDF. Document")
				privateData(PdfName.Create("Version")) = PdfString.Create("The latest version")

				' Specify date of the last modification of "SautinSoft.Pdf" private data (required by PDF specification).
				data(PdfName.Create("LastModified")) = PdfString.Create(DateTimeOffset.Now)

				document.Save("Basic Objects.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Basic Objects.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
