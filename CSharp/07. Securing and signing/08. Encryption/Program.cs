using System;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Security;

class Program
{
    /// <summary>
    /// Encryption.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/Encryption.php
    /// </remarks>
    static void Main()
    {
        Example1();
        Example2();
        Example3();
        Example4();
        Example5();
    }

    static void Example1()
    {
       

        // Load PDF document from an unencrypted PDF file.
        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Set password-based encryption with password required to open a PDF document.
            document.SaveOptions.SetPasswordEncryption().DocumentOpenPassword = "useruser";

            // Save PDF document to an encrypted PDF file.
            document.Save("Encryption.pdf");
        }
    }

    static void Example2()
    {
       

        // Load PDF document from an unencrypted PDF file.
        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Set password-based encryption to an output PDF file.
            var encryption = document.SaveOptions.SetPasswordEncryption();

            // Specify the password required to edit a PDF document.
            encryption.PermissionsPassword = "owner";

            // User will be able to print PDF and fill in the PDF form
            // without requiring a password.
            encryption.Permissions =
                PdfUserAccessPermissions.Print |
                PdfUserAccessPermissions.FillForm |
                PdfUserAccessPermissions.CopyContentForAccessibility |
                PdfUserAccessPermissions.PrintHighResolution;

            // Save the PDF document to an encrypted PDF file.
            document.Save("Restrict Editing.pdf");
        }
    }

    static void Example3()
    {
      

        // Load PDF document from an unencrypted PDF file.
        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Set password-based encryption to an output PDF file.
            var encryption = document.SaveOptions.SetPasswordEncryption();

            // Specify password required to open a PDF document.
            encryption.DocumentOpenPassword = "useruser";

            // Specify password required to edit a PDF document.
            encryption.PermissionsPassword = "owner";

            // User will be able to print PDF and fill-in PDF form 
            // without requiring a password.
            encryption.Permissions =
                PdfUserAccessPermissions.Print |
                PdfUserAccessPermissions.FillForm |
                PdfUserAccessPermissions.CopyContentForAccessibility |
                PdfUserAccessPermissions.PrintHighResolution;

            // Specify 256-bit AES encryption level (supported in Acrobat X and later).
            encryption.EncryptionLevel = new PdfEncryptionLevel(PdfEncryptionAlgorithm.AES, 256);

            // Encrypt content and embedded files but do not encrypt document's metadata.
            encryption.Options = PdfEncryptionOptions.EncryptContent | PdfEncryptionOptions.EncryptEmbeddedFiles;

            // Save PDF document to an encrypted PDF file.
           // document.Save("Encryption Settings.pdf");
        }
    }

    static void Example4()
    {

        try
        {
            // Load PDF document from a potentially encrypted PDF file.
            using (var document = PdfDocument.Load("Encryption.pdf",
                new PdfLoadOptions() { Password = "useruser" }))
            {
                // Remove encryption from an output PDF file.
                document.SaveOptions.Encryption = null;

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
    }

    static void Example5()
    {


        var loadOptions = new PdfLoadOptions();
        loadOptions.AuthorizationOnDocumentOpen = true;

        loadOptions.LoadingEncrypted += (sender, e) =>
        {
            Console.WriteLine("PDF file is encrypted, please enter the password:");
            bool wrongPassword;

            do
            {
                string password = Console.ReadLine();
                if (string.IsNullOrEmpty(password))
                    break;

                wrongPassword = !e.SetPassword(password);
                if (wrongPassword)
                    Console.WriteLine("The password is incorrect, please try again:");
            }
            while (wrongPassword);
        };

        try
        {
            using (var document = PdfDocument.Load("Encryption.pdf", loadOptions))
            {
                Console.WriteLine("The correct password was provided.");
            }
        }
        catch (InvalidPdfPasswordException)
        {
            Console.WriteLine("The incorrect password was provided.");
        }
    }
}
