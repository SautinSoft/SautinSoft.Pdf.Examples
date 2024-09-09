Option Infer On

Imports System
Imports System.IO
Imports System.IO.Compression
Imports System.Linq
Imports SautinSoft.Pdf.Objects
Imports SautinSoft.Pdf.Portfolios
Imports SautinSoft.Pdf

Friend Class Program
	''' <summary>
	''' Create PDF Portfolios.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/create-pdf-portfolios-from-stream.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free 100-day trial key:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = PdfDocument.Load(Path.GetFullPath("..\..\..\PortfolioTemplate.pdf"))
			' Make the document a PDF portfolio (a collection of file attachments).
			Dim portfolio = document.SetPortfolio()

			' Add all files and folders from the zip archive to the portfolio.
			Using archiveStream = File.OpenRead("..\..\..\Attachments.zip")
			Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen:= True)
				For Each entry In archive.Entries
					' Get or create portfolio folder hierarchy from the zip entry full name.
					Dim folder = GetOrAddFolder(portfolio, entry.FullName)

					If Not String.IsNullOrEmpty(entry.Name) Then
						' Zip archive entry is a file.
						Dim files = If(folder Is Nothing, portfolio.Files, folder.Files)

						Dim embeddedFile = files.AddEmpty(entry.Name).EmbeddedFile

						' Set the portfolio file size and modification date.
						If entry.Length < Integer.MaxValue Then
							embeddedFile.Size = CInt(entry.Length)
						End If
						embeddedFile.ModificationDate = entry.LastWriteTime

						' Copy portfolio file contents from the zip archive entry.
						' Portfolio file is compressed if its compressed size in the zip archive is less than its uncompressed size.
						Using entryStream = entry.Open()
						Using embeddedFileStream = embeddedFile.OpenWrite(compress:= entry.CompressedLength < entry.Length)
							entryStream.CopyTo(embeddedFileStream)
						End Using
						End Using
					Else
						' Zip archive entry is a folder.
						' Set the portfolio folder modification date.
						folder.ModificationDate = entry.LastWriteTime
					End If
				Next entry
			End Using
			End Using

			' Set the first PDF file contained in the portfolio to be initially presented in the user interface.
			' Note that all files contained in the portfolio are also contained in the PdfDocument.EmbeddedFiles.
			portfolio.InitialFile = document.EmbeddedFiles.Select(Function(entry) entry.Value).FirstOrDefault(Function(fileSpec) fileSpec.Name.EndsWith(".pdf", StringComparison.Ordinal))

			' Hide all existing portfolio fields except 'Size'.
			For Each portfolioFieldEntry In portfolio.Fields
				portfolioFieldEntry.Value.Hidden = portfolioFieldEntry.Value.Name <> "Size"
			Next portfolioFieldEntry

			' Add a new portfolio field with display name 'Full Name' and it should be in the first column.
			Dim portfolioFieldKeyAndValue = portfolio.Fields.Add(PdfPortfolioFieldDataType.String, "FullName")
			Dim portfolioField = portfolioFieldKeyAndValue.Value
			portfolioField.Name = "Full Name"
			portfolioField.Order = 0

			' For each file and folder in the portfolio, set FullName field value to the relative path of the file/folder in the zip archive.
			SetFullNameFieldValue(portfolio.Files, portfolio.Folders, String.Empty, portfolioFieldKeyAndValue.Key)

			document.Save("Portfolio from Streams.pdf")
		End Using

		System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Portfolio from Streams.pdf") With {.UseShellExecute = True})
	End Sub

	Private Shared Function GetOrAddFolder(ByVal portfolio As PdfPortfolio, ByVal fullName As String) As PdfPortfolioFolder
		Dim folderNames = fullName.Split("/"c)

		Dim folder As PdfPortfolioFolder = Nothing
		Dim folders = portfolio.Folders

		' Last name is the name of the file, so it is skipped.
		For i As Integer = 0 To folderNames.Length - 2
			' Get or add folder with the specific name.
			Dim folderName = folderNames(i)
			folder = folders.FirstOrDefault(Function(f) f.Name = folderName)
			If folder Is Nothing Then
				folder = folders.AddEmpty(folderName)
			End If

			folders = folder.Folders
		Next i

		Return folder
	End Function

	Private Shared Sub SetFullNameFieldValue(ByVal files As PdfPortfolioFileCollection, ByVal folders As PdfPortfolioFolderCollection, ByVal parentFolderFullName As String, ByVal portfolioFieldKey As PdfName)
		' Set FullName field value for all the fields.
		For Each fileSpecification In files
			fileSpecification.PortfolioFieldValues.Add(portfolioFieldKey, New PdfPortfolioFieldValue(parentFolderFullName & fileSpecification.Name))
		Next fileSpecification

		For Each folder In folders
			' Set FullName field value for the folder.
			Dim folderFullName = parentFolderFullName & folder.Name & "/"c
			folder.PortfolioFieldValues.Add(portfolioFieldKey, New PdfPortfolioFieldValue(folderFullName))

			' Recursively set FullName field value for all files and folders underneath the current portfolio folder.
			SetFullNameFieldValue(folder.Files, folder.Folders, folderFullName, portfolioFieldKey)
		Next folder
	End Sub
End Class
