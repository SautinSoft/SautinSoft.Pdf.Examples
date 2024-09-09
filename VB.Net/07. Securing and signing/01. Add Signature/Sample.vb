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
		''' Add sign in PDF
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/add-signature.php
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
				Dim pdfSigner As New PdfSigner("..\..\..\sautinsoft.pfx", "123456789")
				' Configure signer.
				pdfSigner.Timestamper = New PdfTimestamper("https://tsa.cesnet.cz:5817/tsa")
				pdfSigner.SignatureFormat = PdfSignatureFormat.CAdES
				pdfSigner.SignatureLevel = PdfSignatureLevel.PAdES_B_LTA
				pdfSigner.HashAlgorithm = PdfHashAlgorithm.SHA256
				pdfSigner.Location = "Test workplace"
				pdfSigner.Reason = "Test"
				Dim im = PdfImage.Load("..\..\..\JPEG2.jpg")
				sig.Appearance.Icon = im
				sig.Appearance.TextPlacement = PdfTextPlacement.TextRightOfIcon
				' Sign PDF Document.
				Dim si = sig.Sign(pdfSigner)
				' Save PDF Document.
				document.Save()
			End If

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(pdfFile) With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
