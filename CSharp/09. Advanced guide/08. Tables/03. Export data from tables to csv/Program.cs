using System;
using System.IO;
using System.Text.RegularExpressions;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        static void Main(string[] args)
        {
            /// <summary>
            /// Merge PDF files.
            /// </summary>
            /// <remarks>
            /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/export-data-from-table-to-csv.php
            /// </remarks>
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\Item.pdf");
            string csv = "";

            using (var document = PdfDocument.Load(pdfFile))
            {
                var tables = document.Pages[0].Content.FindTables();
                int col = -1;
                double sum = 0;

                foreach (var table in tables) 
                {
                    foreach (var row in table.Rows) 
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            if (col > -1 && i == col)
                            {
                                sum += Convert.ToDouble(row.Cells[i].ToString());
                            }
                            if (row.Cells[i].ToString().Contains("Total Price"))
                            {
                                col = i;
                            }
                            csv += row.Cells[i].ToString() + ';';
                        }
                        csv += "\n";
                    }
                    csv += "Total;;;" + sum.ToString();
                    sum = 0;
                    col = -1;
                    csv += "\n";
                }
            }

            var stream = new FileStream("Output.csv", FileMode.Create);
            stream.Close();
            File.WriteAllText("Output.csv", csv);

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Output.csv") { UseShellExecute = true });
        }
    }
}