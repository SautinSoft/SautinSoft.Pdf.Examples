using System;
using System.Collections.Generic;
using System.IO;
using HarfBuzzSharp;
using Newtonsoft.Json;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Fill PDF form from JSON using C# and .NET.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/fill-pdf-form-from-json-using-csharp-and-dotnet.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 100-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

            // 1. Get json data
            string json = CreateJsonObject();

            // 2. Show JSON in a default viewer.
            string jsonPath = "Cats.json";
            File.WriteAllText(jsonPath, json);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(jsonPath) { UseShellExecute = true });

            // 3. Generate filled PDFs based on "Cat-Template.pdf" and JSON data.
            string resDir = Path.GetFullPath(@"..\..\..\result\");

            // 3.1. Get data from json.            
            var cats = JsonConvert.DeserializeObject<List<CatBreed>>(json);
            string pdfFile = Path.GetFullPath(@"..\..\..\Cat-Template.pdf");
            foreach(var cat in cats)
            {
                var document = PdfDocument.Load(pdfFile);
                var image = PdfImage.Load(cat.PictUrl);
                
                var title = cat.Title;
                document.Form.Fields["TitleTF"].Value = cat.Title;
                document.Form.Fields["DescriptionTF"].Value = cat.Description;
                document.Form.Fields["WeightFromTF"].Value = cat.Weight.Item1.ToString();
                document.Form.Fields["WeightToTF"].Value = cat.Weight.Item2.ToString();
                
                document.Pages[0].Content.DrawImage(image,new PdfPoint(400, 500), new PdfSize(200,200));
                document.Save(Path.Combine(resDir, $"{title}.pdf"));                
            }
            // 4. Show the folder with the resulting PDFs.
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(resDir) { UseShellExecute = true });

        }
        public static string CreateJsonObject()
        {
            string json = String.Empty;
            List<CatBreed> cats = new List<CatBreed>
            {
                new CatBreed() {Title = "Australian Mist",
                    Description = "The Australian Mist (formerly known as the Spotted Mist) is a breed of cat developed in Australia.",
                    PictUrl = "australian-mist.jpg",
                    Weight = (8, 15)},
                new CatBreed() {Title = "Maine Coon",
                    Description = "The Maine Coon is a large domesticated cat breed. It has a distinctive physical appearance and valuable hunting skills.",
                    PictUrl = "maine-coon.png",
                    Weight = (13, 18)},
                new CatBreed() {Title = "Scottish Fold",
                    Description = "The original Scottish Fold was a white barn cat named Susie, who was found at a farm near Coupar Angus in Perthshire, Scotland, in 1961.",
                    PictUrl = "scottish-fold.jpg",
                    Weight = (9, 13)},
                new CatBreed() {Title = "Oriental Shorthair",
                    Description = "The Oriental Shorthair is a breed of domestic cat that is developed from and closely related to the Siamese cat.",
                    PictUrl = "oriental-shorthair.jpg",
                    Weight = (8, 12)},
                new CatBreed() {Title = "Bengal cat",
                    Description = "The earliest mention of an Asian leopard cat × domestic cross was in 1889, when Harrison Weir wrote of them in Our Cats and ...",
                    PictUrl = "bengal-cat.jpg",
                    Weight = (10, 15)},
                new CatBreed() {Title = "Russian Blue",
                    Description = "The Russian Blue is a naturally occurring breed that may have originated in the port of Arkhangelsk in Russia.",
                    PictUrl = "russian-blue.jpg",
                    Weight = (8, 15)},
                new CatBreed() {Title = "Mongrel cat",
                    Description = "A mongrel, mutt or mixed-breed cat is a cat that does not belong to one officially recognized breed, but he's cool and gentle!",
                    PictUrl = "mongrel-cat.jpg",
                    Weight = (8, 16)}
            };

            // Generate full path for the cat's pictures.
            string pictDirectory = Path.GetFullPath(@"..\..\..\picts\");
            foreach (var cb in cats)
            {
                cb.PictUrl = Path.Combine(pictDirectory, cb.PictUrl);
            }

            // Make serialization to JSON format.            
            json = JsonConvert.SerializeObject(cats, new JsonSerializerSettings() { Formatting = Formatting.Indented});
            return json;
        }
    }
    public class CatBreed
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PictUrl { get; set; }
        /// <summary>
        /// Weight in lb. (Fields in template: WeightFrom, WeightTo). Here we are using a tuple.
        /// </summary>
        public (int, int) Weight { get; set; }
    }
}
