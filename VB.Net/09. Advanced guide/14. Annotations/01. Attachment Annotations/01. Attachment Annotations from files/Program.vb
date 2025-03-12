Option Infer On

Imports System.IO
Imports System.IO.Compression
Imports SautinSoft.Pdf.Annotations
Imports SautinSoft.Pdf

Friend Class Program
	''' <summary>
	''' Annotations.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/attachment-annotations-from-file.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = PdfDocument.Load(Path.GetFullPath("..\..\..\simple text.pdf"))
			' Extract all the files in the zip archive to a directory on the file system.
			ZipFile.ExtractToDirectory("..\..\..\Attachments.zip", "Attachments")

			Dim page = document.Pages(0)
			Dim rowCount As Integer = 0
			Dim spacing As Double = page.CropBox.Width \ 5, left As Double = spacing, bottom As Double = page.CropBox.Height - 200

			' Add file attachment annotations to the PDF page from all the files extracted from the zip archive.
			For Each filePath In Directory.GetFiles("Attachments", "*", SearchOption.AllDirectories)
				Dim fileAttachmentAnnotation = page.Annotations.AddFileAttachment(left - 10, bottom - 10, filePath)

				' Set a different icon for each file attachment annotation in a row.
				fileAttachmentAnnotation.Appearance.Icon = CType(rowCount + 1, PdfFileAttachmentIcon)

				' Set attachment description to the relative path of the file in the zip archive.
				fileAttachmentAnnotation.Description = filePath.Substring(filePath.IndexOf("\"c) + 1).Replace("\"c, "/"c)

				' There are, at most, 4 file attachment annotations in a row.
				rowCount += 1
				If rowCount < 4 Then
					left += spacing
				Else
					rowCount = 0
					left = spacing
					bottom -= spacing
				End If
			Next filePath

			' Delete the directory where zip archive files were extracted to.
			Directory.Delete("Attachments", recursive:= True)

			document.Save("File Attachment Annotations from file system.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("File Attachment Annotations from file system.pdf") With {.UseShellExecute = True})
	End Sub
End Class
