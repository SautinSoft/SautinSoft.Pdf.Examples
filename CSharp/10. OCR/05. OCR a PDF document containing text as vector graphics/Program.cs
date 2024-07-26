using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using SautinSoft.Pdf.Objects;
using Tesseract;

namespace OCR
{
    class OCR
    {
        /// <summary>
        /// OCR a PDF document containing text as vector graphics
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/ocr-a-pdf-document-containing-text-as-vector-graphics.php
        /// </remarks>
        static void Main()
        {
            try
            {
                string tesseractLanguages = "eng";
                string tesseractData = Path.GetFullPath(@".\tessdata");
                string tempFile = Path.Combine(tesseractData, Path.GetRandomFileName());
                PdfDocument pdfDocument = PdfDocument.Load(@"..\..\..\Vectorized text.pdf");
                List<MemoryStream> mss = new List<MemoryStream>();
                for (int i = 0; i < 3; i++)
                {
                    MemoryStream ms = new MemoryStream();
                    pdfDocument.Save(ms, new ImageSaveOptions() { PageIndex = i });
                    mss.Add(ms);
                }
                pdfDocument = new PdfDocument();
                PdfFormattedText text = new PdfFormattedText();

                using (Tesseract.TesseractEngine engine = new Tesseract.TesseractEngine(tesseractData, tesseractLanguages, Tesseract.EngineMode.Default))
                {
                    foreach (var ms in mss)
                    {
                        var pdfPage = pdfDocument.Pages.Add();
                        engine.DefaultPageSegMode = Tesseract.PageSegMode.Auto;
                        {
                            byte[] imgBytes = ms.ToArray();
                            using (Tesseract.Pix img = Tesseract.Pix.LoadFromMemory(imgBytes))
                            {
                                using (var page = engine.Process(img, "Serachablepdf"))
                                {
                                    var st = page.GetText();
                                    double scale = Math.Min(pdfPage.Size.Width / page.RegionOfInterest.Width, pdfPage.Size.Height / page.RegionOfInterest.Height);

                                    using (var iter = page.GetIterator())
                                    {
                                        iter.Begin();

                                        do
                                        {
                                            do
                                            {
                                                do
                                                {
                                                    iter.TryGetBoundingBox(PageIteratorLevel.TextLine, out Rect liRect);
                                                    text.FontSize = liRect.Height * scale;
                                                    //text.Opacity = 0;
                                                    text.Append(iter.GetText(PageIteratorLevel.TextLine));
                                                    pdfPage.Content.DrawText(text, new PdfPoint(liRect.X1 * scale, pdfPage.Size.Height - liRect.Y1 * scale - text.Height));
                                                    text.Clear();
                                                } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                            } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                                        } while (iter.Next(PageIteratorLevel.Block));
                                    }
                                }
                            }
                        }
                    }
                }
                pdfDocument.Save(@"text.docx");
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(@"text.docx") { UseShellExecute = true });
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