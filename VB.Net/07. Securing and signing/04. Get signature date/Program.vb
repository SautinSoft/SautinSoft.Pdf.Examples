Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Forms

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' How to determine the signing date of a digital signature on a current pdf using C# and .NET
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/determine-the-signing-date-of-a-digital-signature-on-a-current-pdf-csharp-dotnet.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim inpFile As String = Path.GetFullPath("..\..\..\digital signature.pdf")

			Using document As PdfDocument = PdfDocument.Load(inpFile)
				Dim field = document.Form.Fields.FirstOrDefault(Function(f) TypeOf f Is PdfSignatureField)
				Dim tempVar As Boolean = TypeOf field Is PdfSignatureField
				Dim signField As PdfSignatureField = If(tempVar, CType(field, PdfSignatureField), Nothing)
				If field IsNot Nothing AndAlso tempVar Then
					Console.WriteLine($"The signature date is {signField.Value.Date}.")
				Else
					Console.WriteLine("The PDF doesn't have a digital signature!")
				End If
			End Using
		End Sub
	End Class
End Namespace
