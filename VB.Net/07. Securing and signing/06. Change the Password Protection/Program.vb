Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Security

Friend Class Program
	''' <summary>
	''' Change the Password Protection.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/change-password-protection.php
	''' </remarks>
	Shared Sub Main()
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");
		Try
			' Load PDF document from a potentially encrypted PDF file.
			Using document = PdfDocument.Load(Path.GetFullPath("..\..\..\hello.pdf"), New PdfLoadOptions() With {.Password = "123456"})
				' Remove encryption from an output PDF file.
				document.SaveOptions.Encryption = Nothing

				' Set password-based encryption with password required to open a PDF document.
				document.SaveOptions.SetPasswordEncryption().DocumentOpenPassword = "654321"

				' Save PDF document to an unencrypted PDF file.
				document.Save("Decryption.pdf")
			End Using
		Catch ex As InvalidPdfPasswordException
			' Gracefully handle the case when input PDF file is encrypted 
			' and provided password is invalid.
			Console.WriteLine(ex.Message)
		End Try
		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Decryption.pdf") With {.UseShellExecute = True})
	End Sub
End Class
