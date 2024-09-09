Option Infer On

Imports System
Imports System.IO
Imports System.IO.Compression
Imports SautinSoft.Pdf.Objects
Imports SautinSoft.Pdf.Portfolios
Imports SautinSoft.Pdf

Friend Class Program
	''' <summary>
	''' Create PDF Portfolios.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/extract-portfolios.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free 100-day trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		' Add to zip archive all files and folders from a PDF portfolio.
		Using document = PdfDocument.Load(Path.GetFullPath("..\..\..\Portfolio.pdf"))
		Using archiveStream = File.Create("Portfolio Files and Folders.zip")
		Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen:= True)
			Dim portfolio = document.Portfolio
			If portfolio IsNot Nothing Then
				ExtractFilesAndFoldersToArchive(portfolio.Files, portfolio.Folders, archive, String.Empty, PdfName.Create("FullName"))
			End If
		End Using
		End Using
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Portfolio Files and Folders.zip") With {.UseShellExecute = True})
	End Sub

	Private Shared Sub ExtractFilesAndFoldersToArchive(ByVal files As PdfPortfolioFileCollection, ByVal folders As PdfPortfolioFolderCollection, ByVal archive As ZipArchive, ByVal parentFolderFullName As String, ByVal portfolioFieldKey As PdfName)
		For Each fileSpecification In files
			' Use the FullName field value or the resolved full name as the relative path of the entry in the zip archive.
			Dim entryFullName As String
			Dim fullNameValue As PdfPortfolioFieldValue
			If fileSpecification.PortfolioFieldValues.TryGet(portfolioFieldKey, fullNameValue) Then
				entryFullName = fullNameValue.ToString()
			Else
				entryFullName = parentFolderFullName & fileSpecification.Name
			End If

			Dim embeddedFile = fileSpecification.EmbeddedFile

			' Create zip archive entry.
			' Zip archive entry is compressed if the portfolio embedded file's compressed size is less than its uncompressed size.
			Dim compress As Boolean = embeddedFile.Size Is Nothing OrElse embeddedFile.CompressedSize < embeddedFile.Size.GetValueOrDefault()
			Dim entry = archive.CreateEntry(entryFullName,If(compress, CompressionLevel.Optimal, CompressionLevel.NoCompression))

			' Set the modification date, if it is specified in the portfolio embedded file.
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
		Next fileSpecification

		For Each folder In folders
			' Use the FullName field value or the resolved full name as the relative path of the entry in the zip archive.
			Dim folderFullName As String
			Dim fullNameValue As PdfPortfolioFieldValue
			If folder.PortfolioFieldValues.TryGet(portfolioFieldKey, fullNameValue) Then
				folderFullName = fullNameValue.ToString()
			Else
				folderFullName = parentFolderFullName & folder.Name & "/"c
			End If

			' Set the modification date, if it is specified in the portfolio folder.
			Dim modificationDate = folder.ModificationDate
			If modificationDate.HasValue Then
				archive.CreateEntry(folderFullName).LastWriteTime = modificationDate.GetValueOrDefault()
			End If

			' Recursively add to zip archive all files and folders underneath the current portfolio folder.
			ExtractFilesAndFoldersToArchive(folder.Files, folder.Folders, archive, folderFullName, portfolioFieldKey)
		Next folder
	End Sub
End Class
