Option Infer On

Imports System
Imports System.IO
Imports System.Reflection.Metadata
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Convert PDF to PDF/A FacturX using C# and .NET.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-pdf-to-pdfa-using-csharp-and-dotnet.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");
			Dim inpFile As String = "..\..\..\Factur.rtf"
			Dim outFile As String = "..\..\..\Factur.pdf"
			Dim xmlInfo As String = File.ReadAllText("..\..\..\Factur\Facture.xml")
			' Load a PDF document.
			Using document = PdfDocument.Load(Path.GetFullPath(inpFile))
				' Create PDF save options.
				Dim pdfOptions = New PdfSaveOptions() With {
					.Version = PdfVersion.FacturX,
					.FacturXXml = File.ReadAllText(xmlInfo)
				}

				' Save a PDF document like the FacturX Zugferd.
				' Read more information about Factur-X: https://fnfe-mpe.org/factur-x/

				document.Save(outFile, pdfOptions)
			End Using
		End Sub
	End Class
End Namespace
