Option Infer On

Imports System
Imports System.IO
Imports Org.BouncyCastle.X509
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Forms

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Read digital signature properties from a PDF document using C# and .NET
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/read-digital-signature-properties-from-a-pdf-document-using-csharp-dotnet.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim inpFile As String = Path.GetFullPath("..\..\..\digital signature.pdf")

			Using document As PdfDocument = PdfDocument.Load(inpFile)
				Dim field = document.Form.Fields.FirstOrDefault(Function(f) TypeOf f Is PdfSignatureField)
				Dim tempVar As Boolean = TypeOf field Is PdfSignatureField
				Dim signField As PdfSignatureField = If(tempVar, CType(field, PdfSignatureField), Nothing)
				If field IsNot Nothing AndAlso tempVar Then
					Console.WriteLine($"Name: {signField.Value.Name}.")
					Console.WriteLine($"Date: {signField.Value.Date}.")
					Console.WriteLine($"Location: {signField.Value.Location}.")
					Console.WriteLine($"Reason: {signField.Value.Reason}.")
					Console.WriteLine($"ContactInfo: {signField.Value.ContactInfo}.")

					Dim content = signField.Value.Content
					Dim certificate As New X509Certificate(content.SignerCertificate.GetRawData())
					Console.WriteLine("SignerCertificate data:")
					Console.WriteLine(certificate)
				Else
					Console.WriteLine("The PDF doesn't have a digital signature!")
				End If
			End Using
		End Sub
	End Class
End Namespace
