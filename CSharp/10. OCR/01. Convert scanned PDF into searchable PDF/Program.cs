using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Objects;
using System;
using Tesseract;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OCR
{
    class OCR
    {
        /// <summary>
        /// Convert scanned PDF into searchable PDF
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/convert-scanned-pdf-into-searchable-pdf.php
        /// </remarks>
        static void Main()
        {
            try
            {
                string tesseractLanguages = "eng";
                string tesseractData = Path.GetFullPath(@".\tessdata\");
                string tempFile = Path.Combine(tesseractData, Path.GetRandomFileName());
                PdfDocument pdfDocument = PdfDocument.Load(@"..\..\..\Scanned.pdf");
                PdfFormattedText text = new PdfFormattedText();

                using (Tesseract.TesseractEngine engine = new Tesseract.TesseractEngine(tesseractData, tesseractLanguages, Tesseract.EngineMode.Default))
                {
                    foreach (var pdfPage in pdfDocument.Pages)
                    {
                        var collection = pdfPage.Content.Elements.All().OfType<PdfImageContent>().ToList();
                        for (int i = 0; i < collection.Count(); i++)
                        {
                            engine.DefaultPageSegMode = Tesseract.PageSegMode.Auto;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                collection[i].Save(ms, new ImageSaveOptions());

                                byte[] imgBytes = ms.ToArray();
                                using (Tesseract.Pix img = Tesseract.Pix.LoadFromMemory(imgBytes))
                                {
                                    using (var page = engine.Process(img, "Serachablepdf"))
                                    {
                                        var st = page.GetText();
                                        double scale = Math.Min(collection[i].Bounds.Width / page.RegionOfInterest.Width, collection[i].Bounds.Height / page.RegionOfInterest.Height);

                                        using (var iter = page.GetIterator())
                                        {
                                            iter.Begin();

                                            do
                                            {
                                                do
                                                {
                                                    do
                                                    {
                                                        do
                                                        {
                                                            iter.TryGetBoundingBox(PageIteratorLevel.Word, out Rect liRect);
                                                            text.FontSize = liRect.Height * scale;
                                                            text.Opacity = 0;
                                                            text.Append(iter.GetText(PageIteratorLevel.Word));
                                                            pdfPage.Content.DrawText(text, new PdfPoint(collection[i].Bounds.Left + liRect.X1 * scale, collection[i].Bounds.Top - liRect.Y1 * scale - text.Height));
                                                            text.Clear();
                                                        } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));
                                                    } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                                } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                                            } while (iter.Next(PageIteratorLevel.Block));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                pdfDocument.Save(@"Result.pdf");
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Result.pdf") { UseShellExecute = true });
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine("Please be sure that you have Language data files (*.traineddata) in your folder \"tessdata\"");
                Console.WriteLine("The Language data files can be download from here: https://github.com/tesseract-ocr/tessdata_fast");
                Console.ReadKey();
                throw new Exception("Error Tesseract: " + e.Message);
            }
            finally
            {

            }
        }
    }
}
