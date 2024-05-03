using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Portfolios;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Create PDF Portfolios.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/create-pdf-portfolios-from-files.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\PortfolioTemplate.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                // Make the document a PDF portfolio (a collection of file attachments).
                var portfolio = document.SetPortfolio();

                // Extract all the files in the zip archive to a directory on the file system.
                ZipFile.ExtractToDirectory(@"..\..\..\Attachments.zip", "Attachments");

                // Add files contained directly in the 'Attachments' directory to the portfolio files.
                foreach (var filePath in Directory.GetFiles("Attachments", "*", SearchOption.TopDirectoryOnly))
                    portfolio.Files.Add(filePath);

                // Recursively add directories and their files contained in the 'Attachments' directory to the portfolio folders.
                foreach (var folderPath in Directory.GetDirectories("Attachments", "*", SearchOption.TopDirectoryOnly))
                    portfolio.Folders.Add(folderPath, recursive: true);

                // Delete the directory where zip archive files were extracted to.
                Directory.Delete("Attachments", recursive: true);

                // Set the first PDF file contained in the portfolio to be initially presented in the user interface.
                // Note that all files contained in the portfolio are also contained in the PdfDocument.EmbeddedFiles.
                portfolio.InitialFile = document.EmbeddedFiles.Select(entry => entry.Value).FirstOrDefault(fileSpec => fileSpec.Name.EndsWith(".pdf", StringComparison.Ordinal));

                // Hide all existing portfolio fields except 'Size'.
                foreach (var portfolioFieldEntry in portfolio.Fields)
                    portfolioFieldEntry.Value.Hidden = portfolioFieldEntry.Value.Name != "Size";

                // Add a new portfolio field with display name 'Full Name' and it should be in the first column.
                var portfolioFieldKeyAndValue = portfolio.Fields.Add(PdfPortfolioFieldDataType.Name, "FullName");
                var portfolioField = portfolioFieldKeyAndValue.Value;
                portfolioField.Name = "Full Name";
                portfolioField.Order = 0;

                document.Save("Portfolio from file system.pdf");
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Portfolio from file system.pdf") { UseShellExecute = true });
        }
    }
}