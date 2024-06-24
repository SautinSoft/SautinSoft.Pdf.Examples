using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Forms;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// How to determine the signing date of a digital signature on a current pdf using C# and .NET
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/determine-the-signing-date-of-a-digital-signature-on-a-current-pdf-csharp-dotnet.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string inpFile = Path.GetFullPath(@"..\..\..\digital signature.pdf");
            
            using (PdfDocument document = PdfDocument.Load(inpFile))
            {
                var field = document.Form.Fields.FirstOrDefault(f => f is PdfSignatureField);
                if (field != null && field is PdfSignatureField signField)
                {
                    Console.WriteLine($"The signature date is {signField.Value.Date}.");
                }
                else
                {
                    Console.WriteLine("The PDF doesn't have a digital signature!");
                }
            }
        }
    }
}