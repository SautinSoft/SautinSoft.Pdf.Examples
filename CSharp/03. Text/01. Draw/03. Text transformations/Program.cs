using System;
using SautinSoft.Pdf;
using System.IO;
using SautinSoft.Pdf.Content;

class Program
{
    static void Main()
    {
                    /// <summary>
                    /// Create a page tree.
                    /// </summary>
                    /// <remarks>
                    /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/text-transformations.php
                    /// </remarks>
        // Before starting this example, please get a free 30-day trial key:
        // https://sautinsoft.com/start-for-free/

        // Apply the key here:
        // PdfDocument.SetLicense("...");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            using (var formattedText = new PdfFormattedText())
            {
                var text = "Rotated by 30 degrees around origin.";
                formattedText.Opacity = 0.2;
                formattedText.Append(text);
                var origin = new PdfPoint(50, 650);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.Append(text);
                var transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Rotate(30);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();
                text = "Rotated by 30 degrees around center.";
                formattedText.Opacity = 0.2;
                formattedText.Append(text);
                origin = new PdfPoint(300, 650);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.Append(text);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Rotate(30, formattedText.Width / 2, formattedText.Height / 2);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Scaled horizontally by 0.5 around origin.";
                formattedText.Opacity = 0.2;
                formattedText.Append(text);
                origin = new PdfPoint(50, 500);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.Append(text);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Scale(0.5, 1);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Scaled horizontally by 0.5 around center.";
                formattedText.Opacity = 0.2;
                formattedText.Append(text);
                origin = new PdfPoint(300, 500);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.Append(text);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Scale(0.5, 1, formattedText.Width / 2, formattedText.Height / 2);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Scaled vertically by 2 around origin.";
                formattedText.Opacity = 0.2;
                formattedText.Append(text);
                origin = new PdfPoint(50, 400);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.Append(text);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Scale(1, 2);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Scaled vertically by 2 around center.";
                formattedText.Opacity = 0.2;
                formattedText.Append(text);
                origin = new PdfPoint(300, 400);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.Append(text);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Scale(1, 2, formattedText.Width / 2, formattedText.Height / 2);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Rotated by 30 degrees around origin and ";
                var text2 = "scaled horizontally by 0.5 and ";
                var text3 = "vertically by 2 around origin.";
                formattedText.Opacity = 0.2;
                formattedText.AppendLine(text).AppendLine(text2).Append(text3);
                origin = new PdfPoint(50, 200);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.AppendLine(text).AppendLine(text2).Append(text3);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Rotate(30);
                transform.Scale(0.5, 2);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Rotated by 30 degrees around center and ";
                text2 = "scaled horizontally by 0.5 and ";
                text3 = "vertically by 2 around center.";
                formattedText.Opacity = 0.2;
                formattedText.AppendLine(text).AppendLine(text2).Append(text3);
                origin = new PdfPoint(300, 200);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.AppendLine(text).AppendLine(text2).Append(text3);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Rotate(30, formattedText.Width / 2, formattedText.Height / 2);
                transform.Scale(0.5, 2, formattedText.Width / 2, formattedText.Height / 2);
                page.Content.DrawText(formattedText, transform);
            }
            document.Save("Transformations.pdf");
        }
    }
}