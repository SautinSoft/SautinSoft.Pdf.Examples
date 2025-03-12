Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Get and set PDF viewer preferences.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/viewer-preferences.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")

			Using document = PdfDocument.Load(pdfFile)
				' Get viewer preferences specifying the way the document should be displayed on the screen.
				Dim viewerPreferences = document.ViewerPreferences

				' Modify viewer preferences.
				viewerPreferences.CenterWindow = False
				viewerPreferences.FitWindow = True
				viewerPreferences.HideMenubar = True
				viewerPreferences.HideToolbar = True
				viewerPreferences.NonFullScreenPageMode = PdfPageMode.FullScreen
				viewerPreferences.ViewArea = PdfPageBoundaryType.MediaBox

				document.Save("Viewer Preferences.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Viewer Preferences.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
