Option Infer On

Imports System
Imports System.IO
Imports System.IO.Compression
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Embed files to PDF document.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/embed-files-to-pdf-document.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			Dim pdfFile As String = Path.GetFullPath("..\..\..\simple text.pdf")

			Using document = PdfDocument.Load(pdfFile)
				' Make Attachments panel visible.
				document.PageMode = PdfPageMode.UseAttachments

				' Extract all the files in the zip archive to a directory on the file system.
				ZipFile.ExtractToDirectory("..\..\..\Attachments.zip", "Attachments")

				' Embed in the PDF document all the files extracted from the zip archive.
				For Each filePath In Directory.GetFiles("Attachments", "*", SearchOption.AllDirectories)
					Dim fileSpecification = document.EmbeddedFiles.Add(filePath).Value

					' Set embedded file description to the relative path of the file in the zip archive.
					fileSpecification.Description = filePath.Substring(filePath.IndexOf("\"c) + 1).Replace("\"c, "/"c)
				Next filePath

				' Delete the directory where zip archive files were extracted to.
				Directory.Delete("Attachments", recursive:= True)

				document.Save("Embedded Files from file system.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Embedded Files from file system.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
