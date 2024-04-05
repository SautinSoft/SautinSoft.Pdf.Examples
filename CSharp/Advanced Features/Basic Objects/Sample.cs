using System;
using System.Globalization;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Objects;
using SautinSoft.Pdf.Text;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Use basic PDF objects for currently unsupported PDF features.
        /// </summary>
        /// <remarks>
        /// Details: http://sautinsoft/products/pdf/help/net/developer-guide/basic-objects.php
        /// </remarks>
        static void Main(string[] args)
        {
            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                // Get document's trailer dictionary.
                var trailer = document.GetDictionary();
                // Get document catalog dictionary from the trailer.
                var catalog = (PdfDictionary)((PdfIndirectObject)trailer[PdfName.Create("Root")]).Value;

                // Either retrieve "PieceInfo" entry value from document catalog or create a page-piece dictionary and set it to document catalog under "PieceInfo" entry.
                PdfDictionary pieceInfo;
                var pieceInfoKey = PdfName.Create("PieceInfo");
                var pieceInfoValue = catalog[pieceInfoKey];
                switch (pieceInfoValue.ObjectType)
                {
                    case PdfBasicObjectType.Dictionary:
                        pieceInfo = (PdfDictionary)pieceInfoValue;
                        break;
                    case PdfBasicObjectType.IndirectObject:
                        pieceInfo = (PdfDictionary)((PdfIndirectObject)pieceInfoValue).Value;
                        break;
                    case PdfBasicObjectType.Null:
                        pieceInfo = PdfDictionary.Create();
                        catalog[pieceInfoKey] = PdfIndirectObject.Create(pieceInfo);
                        break;
                    default:
                        throw new InvalidOperationException("PieceInfo entry must be dictionary.");
                }

                // Create page-piece data dictionary for "SautinSoft.Pdf" conforming product and set it to page-piece dictionary.
                var data = PdfDictionary.Create();
                pieceInfo[PdfName.Create("SautinSoft.Pdf")] = data;

                // Create a private data dictionary that will hold private data that "SautinSoft.Pdf" conforming product understands.
                var privateData = PdfDictionary.Create();
                data[PdfName.Create("Data")] = privateData;

                // Set "Title" and "Version" entries to private data.
                privateData[PdfName.Create("Title")] = PdfString.Create("SautinSoft PDF. Document");
                privateData[PdfName.Create("Version")] = PdfString.Create("The latest version");

                // Specify date of the last modification of "SautinSoft.Pdf" private data (required by PDF specification).
                data[PdfName.Create("LastModified")] = PdfString.Create(DateTimeOffset.Now);

                document.Save("Basic Objects.pdf");
            }
        }
    }
}
