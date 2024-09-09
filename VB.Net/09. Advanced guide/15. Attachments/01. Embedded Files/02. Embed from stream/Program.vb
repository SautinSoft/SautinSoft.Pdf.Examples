Option Infer On

Imports System.IO
Imports System.IO.Compression
Imports SautinSoft.Pdf

Friend Class Program
	''' <summary>
	''' Embed files to PDF document.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/embed-files-from-stream-to-pdf-document.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free 100-day trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = PdfDocument.Load(Path.GetFullPath("..\..\..\simple text.pdf"))
			' Make Attachments panel visible.
			document.PageMode = PdfPageMode.UseAttachments

			' Embed in the PDF document all the files from the zip archive.
			Using archiveStream = File.OpenRead("..\..\..\Attachments.zip")
			Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen:= True)
				For Each entry In archive.Entries
					If Not String.IsNullOrEmpty(entry.Name) Then
						Dim fileSpecification = document.EmbeddedFiles.AddEmpty(entry.Name).Value

						' Set embedded file description to the relative path of the file in the zip archive.
						fileSpecification.Description = entry.FullName

						Dim embeddedFile = fileSpecification.EmbeddedFile

						' Set the embedded file size and modification date.
						If entry.Length < Integer.MaxValue Then
							embeddedFile.Size = CInt(entry.Length)
						End If
						embeddedFile.ModificationDate = entry.LastWriteTime

						' Copy embedded file contents from the zip archive entry.
						' Embedded file is compressed if its compressed size in the zip archive is less than its uncompressed size.
						Using entryStream = entry.Open()
						Using embeddedFileStream = embeddedFile.OpenWrite(compress:= entry.CompressedLength < entry.Length)
							entryStream.CopyTo(embeddedFileStream)
						End Using
						End Using
					End If
				Next entry
			End Using
			End Using

			document.Save("Embedded Files from Streams.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Embedded Files from Streams.pdf") With {.UseShellExecute = True})
	End Sub
End Class
