Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Annotations
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Forms
Imports SautinSoft.Pdf.Security

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Add multisign in PDF.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/add-multi-signature.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")

			Dim document = PdfDocument.Load(pdfFile)
			If True Then
				' Add a signature field.
				Dim sig = document.Form.Fields.AddSignature(document.Pages(0), 10, 10, 250, 50)
				' Create new Signer.
				Dim pdfSigner As New PdfSigner("..\..\..\Oliver Ekman.pfx", "1234567890")
				' Configure signer.
				pdfSigner.Timestamper = New PdfTimestamper("https://tsa.cesnet.cz:5817/tsa")
				pdfSigner.SignatureFormat = PdfSignatureFormat.CAdES
				pdfSigner.SignatureLevel = PdfSignatureLevel.PAdES_B_LTA
				pdfSigner.HashAlgorithm = PdfHashAlgorithm.SHA256
				pdfSigner.AuthorPermission = PdfUserAccessPermissions.CommentAndFillForm
				pdfSigner.Location = "Test workplace"
				pdfSigner.Reason = "Test"
				Dim im = PdfImage.Load("..\..\..\JPEG1.jpg")
				sig.Appearance.Icon = im
				sig.Appearance.TextPlacement = PdfTextPlacement.TextRightOfIcon
				' Sign PDF Document.
				sig.Sign(pdfSigner)
				' Save the PDF document. Saving is required to add the next signature. 
				document.Save()
				' Add a new signature field.
				sig = document.Form.Fields.AddSignature(document.Pages(1), 10, 10, 100, 50)
				' Create new Signer.
				pdfSigner = New PdfSigner("..\..\..\sautinsoft.pfx", "123456789")
				' Configure signer.
				pdfSigner.Timestamper = New PdfTimestamper("https://freetsa.org/tsr")
				pdfSigner.SignatureFormat = PdfSignatureFormat.CAdES
				pdfSigner.SignatureLevel = PdfSignatureLevel.PAdES_B_LTA
				pdfSigner.HashAlgorithm = PdfHashAlgorithm.SHA256
				pdfSigner.Location = "Test workplace"
				pdfSigner.Reason = "Test"
				im = PdfImage.Load("..\..\..\JPEG2.jpg")
				sig.Appearance.Icon = im
				sig.Appearance.TextPlacement = PdfTextPlacement.IconOnly
				' Sign the PDF document with another signature.
				sig.Sign(pdfSigner)
				' Save the PDF document.
				document.Save()
			End If

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(pdfFile) With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
