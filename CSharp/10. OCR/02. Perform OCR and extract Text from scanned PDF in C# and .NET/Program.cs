using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;
using Tesseract;

namespace OCR
{
    class OCR
    {
        static void Main()
        {
            try
            {
                string tesseractLanguages = "eng";
                string tesseractData = Path.GetFullPath(@".\tessdata");
                string tempFile = Path.Combine(tesseractData, Path.GetRandomFileName());
                PdfDocument pdfDocument = PdfDocument.Load(@"..\..\..\Scanned PDF 1.pdf");

                using (Tesseract.TesseractEngine engine = new Tesseract.TesseractEngine(tesseractData, tesseractLanguages, Tesseract.EngineMode.Default))
                {
                    int k = 1;
                    foreach (var pdfPage in pdfDocument.Pages)
                    {
                        Console.WriteLine("<Page " + (k++) + ">");
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
                                        var im = PdfImage.Load(@"C:\Users\nekit\Downloads\image_2024_06_25T07_13_20_689Z.png");

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
                                                            Console.Write(iter.GetText(PageIteratorLevel.Word));
                                                            Console.Write(' ');

                                                            if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                                            {
                                                                Console.WriteLine();
                                                            }
                                                        } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                                        if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                                        {
                                                            Console.WriteLine();
                                                        }
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