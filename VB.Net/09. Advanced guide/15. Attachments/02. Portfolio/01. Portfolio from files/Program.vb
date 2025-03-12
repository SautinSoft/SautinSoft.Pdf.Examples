Option Infer On

Imports System
Imports System.IO
Imports System.IO.Compression
Imports System.Linq
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Portfolios

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Create PDF Portfolios.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/create-pdf-portfolios-from-files.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\PortfolioTemplate.pdf")

			Using document = PdfDocument.Load(pdfFile)
				' Make the document a PDF portfolio (a collection of file attachments).
				Dim portfolio = document.SetPortfolio()

				' Extract all the files in the zip archive to a directory on the file system.
				ZipFile.ExtractToDirectory("..\..\..\Attachments.zip", "Attachments")

				' Add files contained directly in the 'Attachments' directory to the portfolio files.
				For Each filePath In Directory.GetFiles("Attachments", "*", SearchOption.TopDirectoryOnly)
					portfolio.Files.Add(filePath)
				Next filePath

				' Recursively add directories and their files contained in the 'Attachments' directory to the portfolio folders.
				For Each folderPath In Directory.GetDirectories("Attachments", "*", SearchOption.TopDirectoryOnly)
					portfolio.Folders.Add(folderPath, recursive:= True)
				Next folderPath

				' Delete the directory where zip archive files were extracted to.
				Directory.Delete("Attachments", recursive:= True)

				' Set the first PDF file contained in the portfolio to be initially presented in the user interface.
				' Note that all files contained in the portfolio are also contained in the PdfDocument.EmbeddedFiles.
				portfolio.InitialFile = document.EmbeddedFiles.Select(Function(entry) entry.Value).FirstOrDefault(Function(fileSpec) fileSpec.Name.EndsWith(".pdf", StringComparison.Ordinal))

				' Hide all existing portfolio fields except 'Size'.
				For Each portfolioFieldEntry In portfolio.Fields
					portfolioFieldEntry.Value.Hidden = portfolioFieldEntry.Value.Name <> "Size"
				Next portfolioFieldEntry

				' Add a new portfolio field with display name 'Full Name' and it should be in the first column.
				Dim portfolioFieldKeyAndValue = portfolio.Fields.Add(PdfPortfolioFieldDataType.Name, "FullName")
				Dim portfolioField = portfolioFieldKeyAndValue.Value
				portfolioField.Name = "Full Name"
				portfolioField.Order = 0

				document.Save("Portfolio from file system.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Portfolio from file system.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
