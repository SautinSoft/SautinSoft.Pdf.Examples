Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Security

Friend Class Program
	''' <summary>
	''' Encryption.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/Encryption.php
	''' </remarks>
	Shared Sub Main()
		Example1()
		Example2()
		Example3()
		Example4()
		Example5()
	End Sub

	Private Shared Sub Example1()


		' Load PDF document from an unencrypted PDF file.
		Using document = PdfDocument.Load("Reading.pdf")
			' Set password-based encryption with password required to open a PDF document.
			document.SaveOptions.SetPasswordEncryption().DocumentOpenPassword = "useruser"

			' Save PDF document to an encrypted PDF file.
			document.Save("Encryption.pdf")
		End Using
	End Sub

	Private Shared Sub Example2()


		' Load PDF document from an unencrypted PDF file.
		Using document = PdfDocument.Load("Reading.pdf")
			' Set password-based encryption to an output PDF file.
			Dim encryption = document.SaveOptions.SetPasswordEncryption()

			' Specify the password required to edit a PDF document.
			encryption.PermissionsPassword = "owner"

			' User will be able to print PDF and fill in the PDF form
			' without requiring a password.
			encryption.Permissions = PdfUserAccessPermissions.Print Or PdfUserAccessPermissions.FillForm Or PdfUserAccessPermissions.CopyContentForAccessibility Or PdfUserAccessPermissions.PrintHighResolution

			' Save the PDF document to an encrypted PDF file.
			document.Save("Restrict Editing.pdf")
		End Using
	End Sub

	Private Shared Sub Example3()


		' Load PDF document from an unencrypted PDF file.
		Using document = PdfDocument.Load("Reading.pdf")
			' Set password-based encryption to an output PDF file.
			Dim encryption = document.SaveOptions.SetPasswordEncryption()

			' Specify password required to open a PDF document.
			encryption.DocumentOpenPassword = "useruser"

			' Specify password required to edit a PDF document.
			encryption.PermissionsPassword = "owner"

			' User will be able to print PDF and fill-in PDF form 
			' without requiring a password.
			encryption.Permissions = PdfUserAccessPermissions.Print Or PdfUserAccessPermissions.FillForm Or PdfUserAccessPermissions.CopyContentForAccessibility Or PdfUserAccessPermissions.PrintHighResolution

			' Specify 256-bit AES encryption level (supported in Acrobat X and later).
			encryption.EncryptionLevel = New PdfEncryptionLevel(PdfEncryptionAlgorithm.AES, 256)

			' Encrypt content and embedded files but do not encrypt document's metadata.
			encryption.Options = PdfEncryptionOptions.EncryptContent Or PdfEncryptionOptions.EncryptEmbeddedFiles

			' Save PDF document to an encrypted PDF file.
		   ' document.Save("Encryption Settings.pdf");
		End Using
	End Sub

	Private Shared Sub Example4()

		Try
			' Load PDF document from a potentially encrypted PDF file.
			Using document = PdfDocument.Load("Encryption.pdf", New PdfLoadOptions() With {.Password = "useruser"})
				' Remove encryption from an output PDF file.
				document.SaveOptions.Encryption = Nothing

				' Save PDF document to an unencrypted PDF file.
				document.Save("Decryption.pdf")
			End Using
		Catch ex As InvalidPdfPasswordException
			' Gracefully handle the case when input PDF file is encrypted 
			' and provided password is invalid.
			Console.WriteLine(ex.Message)
		End Try
	End Sub

	Private Shared Sub Example5()


		Dim loadOptions = New PdfLoadOptions()
		loadOptions.AuthorizationOnDocumentOpen = True

		AddHandler loadOptions.LoadingEncrypted, Sub(sender, e)
			Console.WriteLine("PDF file is encrypted, please enter the password:")
			Dim wrongPassword As Boolean

			Do
				Dim password As String = Console.ReadLine()
				If String.IsNullOrEmpty(password) Then
					Exit Do
				End If

				wrongPassword = Not e.SetPassword(password)
				If wrongPassword Then
					Console.WriteLine("The password is incorrect, please try again:")
				End If
			Loop While wrongPassword
		End Sub

		Try
			Using document = PdfDocument.Load("Encryption.pdf", loadOptions)
				Console.WriteLine("The correct password was provided.")
			End Using
		Catch e1 As InvalidPdfPasswordException
			Console.WriteLine("The incorrect password was provided.")
		End Try
	End Sub
End Class