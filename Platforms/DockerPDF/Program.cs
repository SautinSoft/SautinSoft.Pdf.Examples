using System;
using System.IO;
using System.Reflection.Metadata;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

class Program
{
    static void Main()
    {
        using (var document = PdfDocument.Load("test.pdf"))
        {
            // Load a PDF document and save to a DOCX file.
            document.Save("test.docx");
            Console.WriteLine("Loaded files - [OK]");
        }
    }
}