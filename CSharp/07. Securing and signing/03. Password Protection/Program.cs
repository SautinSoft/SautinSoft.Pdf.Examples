using System;
using SautinSoft.Pdf;
using System.IO;

class Program
{
    /// <summary>
    /// Fill in PDF interactive forms.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/password-protection.php
    /// </remarks>
    static void Main()
    {
        // This property is necessary only for licensed version.
        //SautinSoft.Pdf.Serial = "XXXXXXXXXXX";

        using (var document = PdfDocument.Load(Path.GetFullPath(@"..\..\..\simple text.pdf")))
        {
            // Set password-based encryption with password required to open a PDF document.
            document.SaveOptions.SetPasswordEncryption().DocumentOpenPassword = "user1234";

            // Save PDF document to an encrypted PDF file.
            document.Save("SautinSoft.pdf");
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("SautinSoft.pdf") { UseShellExecute = true });
    }
}