using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Annotations;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Forms;
using SautinSoft.Pdf.Security;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Add multisign in PDF.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/add-multi-signature.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            var document = PdfDocument.Load(pdfFile);
            {
                // Add a signature field.
                var sig = document.Form.Fields.AddSignature(document.Pages[0], 10, 10, 250, 50);
                // Create new Signer.
                PdfSigner pdfSigner = new PdfSigner(@"..\..\..\Oliver Ekman.pfx", "1234567890");
                // Configure signer.
                pdfSigner.Timestamper = new PdfTimestamper(@"https://tsa.cesnet.cz:5817/tsa");
                pdfSigner.SignatureFormat = PdfSignatureFormat.CAdES;
                pdfSigner.SignatureLevel = PdfSignatureLevel.PAdES_B_LTA;
                pdfSigner.HashAlgorithm = PdfHashAlgorithm.SHA256;
                pdfSigner.AuthorPermission = PdfUserAccessPermissions.CommentAndFillForm;
                pdfSigner.Location = "Test workplace";
                pdfSigner.Reason = "Test";
                var im = PdfImage.Load(@"..\..\..\JPEG1.jpg");
                sig.Appearance.Icon = im;
                sig.Appearance.TextPlacement = PdfTextPlacement.TextRightOfIcon;
                // Sign PDF Document.
                sig.Sign(pdfSigner);
                // Save the PDF document. Saving is required to add the next signature. 
                document.Save();
                // Add a new signature field.
                sig = document.Form.Fields.AddSignature(document.Pages[1], 10, 10, 100, 50);
                // Create new Signer.
                pdfSigner = new PdfSigner(@"..\..\..\sautinsoft.pfx", "123456789");
                // Configure signer.
                pdfSigner.Timestamper = new PdfTimestamper(@"https://freetsa.org/tsr");
                pdfSigner.SignatureFormat = PdfSignatureFormat.CAdES;
                pdfSigner.SignatureLevel = PdfSignatureLevel.PAdES_B_LTA;
                pdfSigner.HashAlgorithm = PdfHashAlgorithm.SHA256;
                pdfSigner.Location = "Test workplace";
                pdfSigner.Reason = "Test";
                im = PdfImage.Load(@"..\..\..\JPEG2.jpg");
                sig.Appearance.Icon = im;
                sig.Appearance.TextPlacement = PdfTextPlacement.IconOnly;
                // Sign the PDF document with another signature.
                sig.Sign(pdfSigner);
                // Save the PDF document.
                document.Save();
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(pdfFile) { UseShellExecute = true });
        }
    }
}
