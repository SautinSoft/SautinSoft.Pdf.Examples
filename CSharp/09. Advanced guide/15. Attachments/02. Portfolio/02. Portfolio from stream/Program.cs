using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SautinSoft.Pdf.Objects;
using SautinSoft.Pdf.Portfolios;
using SautinSoft.Pdf;

class Program
{
    /// <summary>
    /// Create PDF Portfolios.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/create-pdf-portfolios-from-stream.php
    /// </remarks>
    static void Main()
    {
        // Before starting this example, please get a free 100-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        using (var document = PdfDocument.Load(Path.GetFullPath(@"..\..\..\PortfolioTemplate.pdf")))
        {
            // Make the document a PDF portfolio (a collection of file attachments).
            var portfolio = document.SetPortfolio();

            // Add all files and folders from the zip archive to the portfolio.
            using (var archiveStream = File.OpenRead(@"..\..\..\Attachments.zip"))
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen: true))
                foreach (var entry in archive.Entries)
                {
                    // Get or create portfolio folder hierarchy from the zip entry full name.
                    var folder = GetOrAddFolder(portfolio, entry.FullName);

                    if (!string.IsNullOrEmpty(entry.Name))
                    {
                        // Zip archive entry is a file.
                        var files = folder == null ? portfolio.Files : folder.Files;

                        var embeddedFile = files.AddEmpty(entry.Name).EmbeddedFile;

                        // Set the portfolio file size and modification date.
                        if (entry.Length < int.MaxValue)
                            embeddedFile.Size = (int)entry.Length;
                        embeddedFile.ModificationDate = entry.LastWriteTime;

                        // Copy portfolio file contents from the zip archive entry.
                        // Portfolio file is compressed if its compressed size in the zip archive is less than its uncompressed size.
                        using (var entryStream = entry.Open())
                        using (var embeddedFileStream = embeddedFile.OpenWrite(compress: entry.CompressedLength < entry.Length))
                            entryStream.CopyTo(embeddedFileStream);
                    }
                    else
                        // Zip archive entry is a folder.
                        // Set the portfolio folder modification date.
                        folder.ModificationDate = entry.LastWriteTime;
                }

            // Set the first PDF file contained in the portfolio to be initially presented in the user interface.
            // Note that all files contained in the portfolio are also contained in the PdfDocument.EmbeddedFiles.
            portfolio.InitialFile = document.EmbeddedFiles.Select(entry => entry.Value).FirstOrDefault(fileSpec => fileSpec.Name.EndsWith(".pdf", StringComparison.Ordinal));

            // Hide all existing portfolio fields except 'Size'.
            foreach (var portfolioFieldEntry in portfolio.Fields)
                portfolioFieldEntry.Value.Hidden = portfolioFieldEntry.Value.Name != "Size";

            // Add a new portfolio field with display name 'Full Name' and it should be in the first column.
            var portfolioFieldKeyAndValue = portfolio.Fields.Add(PdfPortfolioFieldDataType.String, "FullName");
            var portfolioField = portfolioFieldKeyAndValue.Value;
            portfolioField.Name = "Full Name";
            portfolioField.Order = 0;

            // For each file and folder in the portfolio, set FullName field value to the relative path of the file/folder in the zip archive.
            SetFullNameFieldValue(portfolio.Files, portfolio.Folders, string.Empty, portfolioFieldKeyAndValue.Key);

            document.Save("Portfolio from Streams.pdf");
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Portfolio from Streams.pdf") { UseShellExecute = true });
    }

    static PdfPortfolioFolder GetOrAddFolder(PdfPortfolio portfolio, string fullName)
    {
        var folderNames = fullName.Split('/');

        PdfPortfolioFolder folder = null;
        var folders = portfolio.Folders;

        // Last name is the name of the file, so it is skipped.
        for (int i = 0; i < folderNames.Length - 1; ++i)
        {
            // Get or add folder with the specific name.
            var folderName = folderNames[i];
            folder = folders.FirstOrDefault(f => f.Name == folderName);
            if (folder == null)
                folder = folders.AddEmpty(folderName);

            folders = folder.Folders;
        }

        return folder;
    }

    static void SetFullNameFieldValue(PdfPortfolioFileCollection files, PdfPortfolioFolderCollection folders, string parentFolderFullName, PdfName portfolioFieldKey)
    {
        // Set FullName field value for all the fields.
        foreach (var fileSpecification in files)
            fileSpecification.PortfolioFieldValues.Add(portfolioFieldKey, new PdfPortfolioFieldValue(parentFolderFullName + fileSpecification.Name));

        foreach (var folder in folders)
        {
            // Set FullName field value for the folder.
            var folderFullName = parentFolderFullName + folder.Name + '/';
            folder.PortfolioFieldValues.Add(portfolioFieldKey, new PdfPortfolioFieldValue(folderFullName));

            // Recursively set FullName field value for all files and folders underneath the current portfolio folder.
            SetFullNameFieldValue(folder.Files, folder.Folders, folderFullName, portfolioFieldKey);
        }
    }
}