using SautinSoft.Pdf;
using SautinSoft.Pdf.Forms;
using System;

class Program
{
    static void Main()
    {
       
        using (var document = PdfDocument.Load(@"..\..\Multiple Digital Signature.pdf"))
        {
            foreach (var field in document.Form.Fields)
                if (field.FieldType == PdfFieldType.Signature)
                {
                    var signatureField = (PdfSignatureField)field;

                    var signature = signatureField.Value;

                    if (signature != null)
                    {
                        var signatureValidationResult = signature.Validate();

                        if (signatureValidationResult.IsValid)
                        {
                            Console.Write("Signature '{0}' is VALID, signed by '{1}'. ", signatureField.Name, signature.Content.SignerCertificate.SubjectCommonName);
                            Console.WriteLine("The document has not been modified since this signature was applied.");
                        }
                        else
                        {
                            Console.Write("Signature '{0}' is INVALID. ", signatureField.Name);
                            Console.WriteLine("The document has been altered or corrupted since the signature was applied.");
                        }
                    }
                }
        }
    }
}
