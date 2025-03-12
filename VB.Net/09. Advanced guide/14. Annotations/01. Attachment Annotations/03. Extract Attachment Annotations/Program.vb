Option Infer On

Imports System
Imports System.IO
Imports System.IO.Compression
Imports SautinSoft.Pdf.Annotations
Imports SautinSoft.Pdf

Friend Class Program
	''' <summary>
	''' Annotations.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/extract-attachment-annotations.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		' Add to zip archive all files from file attachment annotations located on the first page.
		Using document = PdfDocument.Load(Path.GetFullPath("..\..\..\File Attachment Annotations.pdf"))
		Using archiveStream = File.Create("File Attachment Annotation Files.zip")
		Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen:= True)
			For Each annotation In document.Pages(0).Annotations
				If annotation.AnnotationType = PdfAnnotationType.FileAttachment Then
					Dim fileAttachmentAnnotation = CType(annotation, PdfFileAttachmentAnnotation)

					Dim fileSpecification = fileAttachmentAnnotation.File

					' Use the description or the file name as the relative path of the entry in the zip archive.
					Dim entryFullName = fileAttachmentAnnotation.Description
					If entryFullName Is Nothing OrElse Not entryFullName.EndsWith(fileSpecification.Name, StringComparison.Ordinal) Then
						entryFullName = fileSpecification.Name
					End If

					Dim embeddedFile = fileSpecification.EmbeddedFile

					' Create zip archive entry.
					' Zip archive entry is compressed if the embedded file's compressed size is less than its uncompressed size.
					Dim compress As Boolean = embeddedFile.Size Is Nothing OrElse embeddedFile.CompressedSize < embeddedFile.Size.GetValueOrDefault()
					Dim entry = archive.CreateEntry(entryFullName,If(compress, CompressionLevel.Optimal, CompressionLevel.NoCompression))

					' Set the modification date, if it is specified in the embedded file.
					Dim modificationDate = embeddedFile.ModificationDate
					If modificationDate IsNot Nothing Then
						entry.LastWriteTime = modificationDate.GetValueOrDefault()
					End If

					' Copy embedded file contents to the zip archive entry.
					Using embeddedFileStream = embeddedFile.OpenRead()
					Using entryStream = entry.Open()
						embeddedFileStream.CopyTo(entryStream)
					End Using
					End Using
				End If
			Next annotation
		End Using
		End Using
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("File Attachment Annotation Files.zip") With {.UseShellExecute = True})
	End Sub
End Class
