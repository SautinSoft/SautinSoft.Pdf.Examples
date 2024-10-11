using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Security;

class Program
{
    /// <summary>
    /// Change the Password Protection.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/change-password-protection.php
    /// </remarks>
    static void Main()
    {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");
        try
        {
            // Load PDF document from a potentially encrypted PDF file.
            using (var document = PdfDocument.Load(Path.GetFullPath(@"..\..\..\hello.pdf"),
                new PdfLoadOptions() { Password = "123456" }))
            {
                // Remove encryption from an output PDF file.
                document.SaveOptions.Encryption = null;

                // Set password-based encryption with password required to open a PDF document.
                document.SaveOptions.SetPasswordEncryption().DocumentOpenPassword = "654321";

                // Save PDF document to an unencrypted PDF file.
                document.Save("Decryption.pdf");
            }
        }
        catch (InvalidPdfPasswordException ex)
        {
            // Gracefully handle the case when input PDF file is encrypted 
            // and provided password is invalid.
            Console.WriteLine(ex.Message);
        }
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Decryption.pdf") { UseShellExecute = true });
    }
}