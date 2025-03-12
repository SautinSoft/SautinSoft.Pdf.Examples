Option Infer On

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports HarfBuzzSharp
Imports Newtonsoft.Json
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Fill PDF form from JSON using C# and .NET.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/fill-pdf-form-from-json-using-csharp-and-dotnet.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...")

			' 1. Get json data
			Dim json As String = CreateJsonObject()

			' 2. Show JSON in a default viewer.
			Dim jsonPath As String = "Cats.json"
			File.WriteAllText(jsonPath, json)
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(jsonPath) With {.UseShellExecute = True})

			' 3. Generate filled PDFs based on "Cat_Template.pdf" and JSON data.
			Dim resDir As String = Path.GetFullPath("..\..\..\result\")
			If Not Directory.Exists(resDir) Then
				Directory.CreateDirectory(resDir)
			End If


			' 3.1. Get data from json.            
			Dim cats = JsonConvert.DeserializeObject(Of List(Of CatBreed))(json)
			Dim pdfFile As String = Path.GetFullPath("..\..\..\Cat_Template.pdf")
			For Each cat In cats
				Dim document = PdfDocument.Load(pdfFile)
				Dim image = PdfImage.Load(cat.PictUrl)

				Dim title = cat.Title
				document.Form.Fields("TitleTF").Value = cat.Title
				document.Form.Fields("DescriptionTF").Value = cat.Description
				document.Form.Fields("WeightFromTF").Value = cat.Weight.Item1.ToString()
				document.Form.Fields("WeightToTF").Value = cat.Weight.Item2.ToString()

				document.Pages(0).Content.DrawImage(image, New PdfPoint(400, 500), New PdfSize(200, 200))
				document.Save(Path.Combine(resDir, $"{title}.pdf"))
			Next cat
			' 4. Show the folder with the resulting PDFs.
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(resDir) With {.UseShellExecute = True})

		End Sub
		Public Shared Function CreateJsonObject() As String
			Dim json As String = String.Empty
			Dim cats As New List(Of CatBreed) From {
				New CatBreed() With {
					.Title = "Australian Mist",
					.Description = "The Australian Mist (formerly known as the Spotted Mist) is a breed of cat developed in Australia.",
					.PictUrl = "australian-mist.jpg",
					.Weight = (8, 15)
				},
				New CatBreed() With {
					.Title = "Maine Coon",
					.Description = "The Maine Coon is a large domesticated cat breed. It has a distinctive physical appearance and valuable hunting skills.",
					.PictUrl = "maine-coon.png",
					.Weight = (13, 18)
				},
				New CatBreed() With {
					.Title = "Scottish Fold",
					.Description = "The original Scottish Fold was a white barn cat named Susie, who was found at a farm near Coupar Angus in Perthshire, Scotland, in 1961.",
					.PictUrl = "scottish-fold.jpg",
					.Weight = (9, 13)
				},
				New CatBreed() With {
					.Title = "Oriental Shorthair",
					.Description = "The Oriental Shorthair is a breed of domestic cat that is developed from and closely related to the Siamese cat.",
					.PictUrl = "oriental-shorthair.jpg",
					.Weight = (8, 12)
				},
				New CatBreed() With {
					.Title = "Bengal cat",
					.Description = "The earliest mention of an Asian leopard cat × domestic cross was in 1889, when Harrison Weir wrote of them in Our Cats and ...",
					.PictUrl = "bengal-cat.jpg",
					.Weight = (10, 15)
				},
				New CatBreed() With {
					.Title = "Russian Blue",
					.Description = "The Russian Blue is a naturally occurring breed that may have originated in the port of Arkhangelsk in Russia.",
					.PictUrl = "russian-blue.jpg",
					.Weight = (8, 15)
				},
				New CatBreed() With {
					.Title = "Mongrel cat",
					.Description = "A mongrel, mutt or mixed-breed cat is a cat that does not belong to one officially recognized breed, but he's cool and gentle!",
					.PictUrl = "mongrel-cat.jpg",
					.Weight = (8, 16)
				}
			}

			' Generate full path for the cat's pictures.
			Dim pictDirectory As String = Path.GetFullPath("..\..\..\picts\")
			For Each cb In cats
				cb.PictUrl = Path.Combine(pictDirectory, cb.PictUrl)
			Next cb

			' Make serialization to JSON format.            
			json = JsonConvert.SerializeObject(cats, New JsonSerializerSettings() With {.Formatting = Formatting.Indented})
			Return json
		End Function
	End Class
	Public Class CatBreed
		Public Property Title As String
		Public Property Description As String
		Public Property PictUrl As String
		''' <summary>
		''' Weight in lb. (Fields in template: WeightFrom, WeightTo). Here we are using a tuple.
		''' </summary>
		Public Property Weight As (Integer, Integer)
	End Class
End Namespace
