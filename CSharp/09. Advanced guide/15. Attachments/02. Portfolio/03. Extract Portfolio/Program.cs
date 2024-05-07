using System;
using System.IO;
using System.IO.Compression;
using SautinSoft.Pdf.Objects;
using SautinSoft.Pdf.Portfolios;
using SautinSoft.Pdf;

class Program
{
    /// <summary>
    /// Create PDF Portfolios.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/extract-portfolios.php
    /// </remarks>
    static void Main()
    {
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        // Add to zip archive all files and folders from a PDF portfolio.
        using (var document = PdfDocument.Load(Path.GetFullPath(@"..\..\..\Portfolio.pdf")))
        using (var archiveStream = File.Create("Portfolio Files and Folders.zip"))
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            var portfolio = document.Portfolio;
            if (portfolio != null)
                ExtractFilesAndFoldersToArchive(portfolio.Files, portfolio.Folders, archive, string.Empty, PdfName.Create("FullName"));
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Portfolio Files and Folders.zip") { UseShellExecute = true });
    }

    static void ExtractFilesAndFoldersToArchive(PdfPortfolioFileCollection files, PdfPortfolioFolderCollection folders, ZipArchive archive, string parentFolderFullName, PdfName portfolioFieldKey)
    {
        foreach (var fileSpecification in files)
        {
            // Use the FullName field value or the resolved full name as the relative path of the entry in the zip archive.
            string entryFullName;
            if (fileSpecification.PortfolioFieldValues.TryGet(portfolioFieldKey, out PdfPortfolioFieldValue fullNameValue))
                entryFullName = fullNameValue.ToString();
            else
                entryFullName = parentFolderFullName + fileSpecification.Name;

            var embeddedFile = fileSpecification.EmbeddedFile;

            // Create zip archive entry.
            // Zip archive entry is compressed if the portfolio embedded file's compressed size is less than its uncompressed size.
            bool compress = embeddedFile.Size == null || embeddedFile.CompressedSize < embeddedFile.Size.GetValueOrDefault();
            var entry = archive.CreateEntry(entryFullName, compress ? CompressionLevel.Optimal : CompressionLevel.NoCompression);

            // Set the modification date, if it is specified in the portfolio embedded file.
            var modificationDate = embeddedFile.ModificationDate;
            if (modificationDate != null)
                entry.LastWriteTime = modificationDate.GetValueOrDefault();

            // Copy embedded file contents to the zip archive entry.
            using (var embeddedFileStream = embeddedFile.OpenRead())
            using (var entryStream = entry.Open())
                embeddedFileStream.CopyTo(entryStream);
        }

        foreach (var folder in folders)
        {
            // Use the FullName field value or the resolved full name as the relative path of the entry in the zip archive.
            string folderFullName;
            if (folder.PortfolioFieldValues.TryGet(portfolioFieldKey, out PdfPortfolioFieldValue fullNameValue))
                folderFullName = fullNameValue.ToString();
            else
                folderFullName = parentFolderFullName + folder.Name + '/';

            // Set the modification date, if it is specified in the portfolio folder.
            var modificationDate = folder.ModificationDate;
            if (modificationDate.HasValue)
                archive.CreateEntry(folderFullName).LastWriteTime = modificationDate.GetValueOrDefault();

            // Recursively add to zip archive all files and folders underneath the current portfolio folder.
            ExtractFilesAndFoldersToArchive(folder.Files, folder.Folders, archive, folderFullName, portfolioFieldKey);
        }
    }
}