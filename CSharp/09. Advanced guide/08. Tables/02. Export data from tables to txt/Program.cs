using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.Json;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Find Tables
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/export-data-from-table-to-txt.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\tables.pdf");
            var writer = new StringWriter(CultureInfo.InvariantCulture);

            using (var document = PdfDocument.Load(pdfFile))
            {
                // Find Tables.
                var tables = document.Pages[0].Content.FindTables();

                string format = "{0,-20}|{1,-20}", separator = new string('-', 40);

                // Get text from tables.
                foreach (var table in tables) 
                {
                    foreach (var row in table.Rows)
                    {
                        writer.WriteLine(format, row.Cells[0].ToString(), row.Cells[1].ToString());
                        writer.WriteLine(separator);
                    }
                    writer.WriteLine();
                }
            }

            var file = new FileStream("Output.txt", FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(file);
            streamWriter.WriteLine(writer.ToString());
            streamWriter.Close();
            file.Close();

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Output.txt") { UseShellExecute = true });
        }
    }
}