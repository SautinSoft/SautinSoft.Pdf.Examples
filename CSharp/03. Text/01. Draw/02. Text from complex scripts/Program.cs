using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content;

class Program
{
    /// <summary>
    /// Create a page tree.
    /// </summary>
    /// <remarks>
    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/text-from-complex-scripts.php
    /// </remarks>
    static void Main()
    {
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        using (var document = new PdfDocument())
        {
            // Create a new page.
            var page = document.Pages.Add();

            using (var formattedText = new PdfFormattedText())
            {
                // Set up and fill a PdfFormattedText object with multilingual text.
                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);

                formattedText.AppendLine("An example of a fully vocalised (vowelised or vowelled) Arabic ").
                Append("from the Basmala: ");

                formattedText.Language = new PdfLanguage("ar-SA");
                formattedText.Font = new PdfFont("Arial", 24);
                formattedText.Append("بِسْمِ ٱللَّٰهِ ٱلرَّحْمَٰنِ ٱلرَّحِيمِ");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.AppendLine(", which means: ").
                Append("In the name of God, the All-Merciful, the Especially-Merciful.");

                // Draw this text.
                page.Content.DrawText(formattedText, new PdfPoint(50, 750));
                // Clear PdfFormattedText object.
                formattedText.Clear();
                // Set up and fill a PdfFormattedText object with multilingual text.
                formattedText.Append("An example of Hebrew: ");

                formattedText.Language = new PdfLanguage("he-IL");
                formattedText.Font = new PdfFont("Arial", 24);
                formattedText.Append("מה קורה");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.AppendLine(", which means: What's going on, ").
                Append("and ");

                formattedText.Language = new PdfLanguage("he-IL");
                formattedText.Font = new PdfFont("Arial", 24);
                formattedText.Append("תודה לכולם");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.Append(", which means: Thank you all.");
                // Draw this text.
                page.Content.DrawText(formattedText, new PdfPoint(50, 650));
                // Clear PdfFormattedText object.
                formattedText.Clear();
                // Set up and fill a PdfFormattedText object with multilingual text.
                formattedText.LineHeight = 50;

                formattedText.Append("An example of Thai: ");
                formattedText.Language = new PdfLanguage("th-TH");
                formattedText.Font = new PdfFont("Leelawadee UI", 16);
                formattedText.AppendLine("ภัำ");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.Append("An example of Tamil: ");
                formattedText.Language = new PdfLanguage("ta-IN");
                formattedText.Font = new PdfFont("Nirmala UI", 16);
                formattedText.AppendLine("போது");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.Append("An example of Bengali: ");
                formattedText.Language = new PdfLanguage("be-IN");
                formattedText.Font = new PdfFont("Nirmala UI", 16);
                formattedText.AppendLine("আবেদনকারীর মাতার পিতার বর্তমান স্থায়ী ঠিকানা নমিনি নাম");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.Append("An example of Gujarati: ");
                formattedText.Language = new PdfLanguage("gu-IN");
                formattedText.Font = new PdfFont("Nirmala UI", 16);
                formattedText.AppendLine("કાર્બન કેમેસ્ટ્રી");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.Append("An example of Osage: ");
                formattedText.Language = new PdfLanguage("osa");
                formattedText.Font = new PdfFont("Gadugi", 16);
                formattedText.Append("𐓏𐓘𐓻𐓘𐓻𐓟 𐒻𐓟");
                // Draw this text.
                page.Content.DrawText(formattedText, new PdfPoint(50, 350));
            }
            // Save PDF Document.
            document.Save("Complex scripts.pdf");
        }
    }
}