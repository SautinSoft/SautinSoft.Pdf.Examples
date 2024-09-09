Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO

Friend Class Program
	''' <summary>
	''' Bookmarks.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/bookmarks.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free 100-day trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = New PdfDocument()
			document.Pages.Add()
			' Remove all bookmarks.
			document.Outlines.Clear()
			' Get the number of pages.
			Dim numberOfPages As Integer = document.Pages.Count
			Dim i As Integer = 0
			Do While i < numberOfPages
				' Add a new outline item (bookmark) at the end of the document outline collection.
				Dim bookmark = document.Outlines.AddLast(String.Format("PAGES {0}-{1}", i + 1, Math.Min(i + 10, numberOfPages)))
				' Set the explicit destination on the new outline item (bookmark).
				bookmark.SetDestination(document.Pages(i), PdfDestinationViewType.FitRectangle, 0, 0, 100, 100)
				Dim j As Integer = 0
				Do While j < Math.Min(10, numberOfPages - i)
					' Add a new outline item (bookmark) at the end of parent outline item (bookmark) and set the explicit destination.
					bookmark.Outlines.AddLast(String.Format("PAGE {0}", i + j + 1)).SetDestination(document.Pages(i + j), PdfDestinationViewType.FitPage)
					j += 1
				Loop
				i += 10
			Loop
			document.PageMode = PdfPageMode.UseOutlines
			document.Save("Bookmarks.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Bookmarks.pdf") With {.UseShellExecute = True})
	End Sub
End Class
