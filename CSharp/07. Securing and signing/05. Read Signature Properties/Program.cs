using System;
using System.IO;
using Org.BouncyCastle.X509;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Forms;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Read digital signature properties from a PDF document using C# and .NET
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/read-digital-signature-properties-from-a-pdf-document-using-csharp-dotnet.php
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
                    Console.WriteLine($"Name: {signField.Value.Name}.");
                    Console.WriteLine($"Date: {signField.Value.Date}.");
                    Console.WriteLine($"Location: {signField.Value.Location}.");
                    Console.WriteLine($"Reason: {signField.Value.Reason}.");
                    Console.WriteLine($"ContactInfo: {signField.Value.ContactInfo}.");

                    var content = signField.Value.Content;
                    X509Certificate certificate = new X509Certificate(content.SignerCertificate.GetRawData());
                    Console.WriteLine("SignerCertificate data:");
                    Console.WriteLine(certificate);
                }
                else
                {
                    Console.WriteLine("The PDF doesn't have a digital signature!");
                }
            }
        }
    }
}