Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO

Friend Class Program
	''' <summary>
	''' Password Protection.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/password-protection.php
	''' </remarks>
	Shared Sub Main()
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

		Using document = PdfDocument.Load(Path.GetFullPath("..\..\..\simple text.pdf"))
			' Set password-based encryption with password required to open a PDF document.
			document.SaveOptions.SetPasswordEncryption().DocumentOpenPassword = "user1234"

			' Save PDF document to an encrypted PDF file.
			document.Save("SautinSoft.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("SautinSoft.pdf") With {.UseShellExecute = True})
	End Sub
End Class
