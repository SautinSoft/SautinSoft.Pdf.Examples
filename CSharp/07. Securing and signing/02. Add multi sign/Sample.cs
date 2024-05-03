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
        static void Main(string[] args)
        {
            /// <summary>
            /// Fill in PDF interactive forms.
            /// </summary>
            /// <remarks>
            /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/add-multi-signature.php
            /// </remarks>
                // Before starting this example, please g
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            var document = PdfDocument.Load(pdfFile);
            {
                var sig = document.Form.Fields.AddSignature(document.Pages[0], 10, 10, 250, 50);
                PdfSigner pdfSigner = new PdfSigner(@"..\..\..\Oliver Ekman.pfx", "1234567890");
                pdfSigner.Timestamper = new PdfTimestamper(@"https://freetsa.org/tsr");
                pdfSigner.SignatureFormat = PdfSignatureFormat.CAdES;
                pdfSigner.SignatureLevel = PdfSignatureLevel.PAdES_B_LTA;
                pdfSigner.HashAlgorithm = PdfHashAlgorithm.SHA256;
                pdfSigner.AuthorPermission = PdfUserAccessPermissions.CommentAndFillForm;
                pdfSigner.Location = "Test workplace";
                pdfSigner.Reason = "Test";
                var im = PdfImage.Load(@"..\..\..\IPEG1.jpg");
                sig.Appearance.Icon = im;
                sig.Appearance.TextPlacement = PdfTextPlacement.TextRightOfIcon;
                sig.Sign(pdfSigner);
                document.Save();
                sig = document.Form.Fields.AddSignature(document.Pages[1], 10, 10, 100, 50);
                pdfSigner = new PdfSigner(@"..\..\..\sautinsoft.pfx", "123456789");
                pdfSigner.Timestamper = new PdfTimestamper(@"https://freetsa.org/tsr");
                pdfSigner.SignatureFormat = PdfSignatureFormat.CAdES;
                pdfSigner.SignatureLevel = PdfSignatureLevel.PAdES_B_LTA;
                pdfSigner.HashAlgorithm = PdfHashAlgorithm.SHA256;
                pdfSigner.Location = "Test workplace";
                pdfSigner.Reason = "Test";
                im = PdfImage.Load(@"..\..\..\JPEG2.jpg");
                sig.Appearance.Icon = im;
                sig.Appearance.TextPlacement = PdfTextPlacement.IconOnly;
                sig.Sign(pdfSigner);

                document.Save();
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(pdfFile) { UseShellExecute = true });
        }
    }
}
