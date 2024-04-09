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
            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            var document = PdfDocument.Load(pdfFile);
            {
                var sig = document.Form.Fields.AddSignature(document.Pages[0], 10, 10, 250, 50);
                PdfSigner pdfSigner = new PdfSigner(@"..\..\..\sautinsoft.pfx", "123456789");
                pdfSigner.Timestamper = new PdfTimestamper(@"https://freetsa.org/tsr");
                pdfSigner.SignatureFormat = PdfSignatureFormat.CAdES;
                pdfSigner.SignatureLevel = PdfSignatureLevel.PAdES_B_LTA;
                pdfSigner.HashAlgorithm = PdfHashAlgorithm.SHA256;
                pdfSigner.Location = "Test workplace";
                pdfSigner.Reason = "Test";
                var im = PdfImage.Load(@"..\..\..\JPEG2.jpg");
                sig.Appearance.Icon = im;
                sig.Appearance.TextPlacement = PdfTextPlacement.TextRightOfIcon;
                var si = sig.Sign(pdfSigner);

                document.Save();
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(pdfFile) { UseShellExecute = true });
        }
    }
}
