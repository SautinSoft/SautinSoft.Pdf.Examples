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
		''' Convert PDF to TIFF in C# and .NET.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-pdf-to-multipage-tiff-using-csharp-and-dotnet.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free license:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			' Load a PDF document.
			Using document = PdfDocument.Load(Path.GetFullPath("..\..\..\Butterfly.pdf"))
				' Create image save options.
				Dim tiffOptions = New ImageSaveOptions() With {
					.Format = ImageSaveFormat.Tiff,
					.PageIndex = 1,
					.PageCount = 3,
					.DpiX = 300,
					.DpiY = 300,
					.PixelFormat = PixelFormat.Rgb24
				}

				' Save a TIFF file.
				document.Save("Result.tiff", tiffOptions)
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Result.tiff") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
