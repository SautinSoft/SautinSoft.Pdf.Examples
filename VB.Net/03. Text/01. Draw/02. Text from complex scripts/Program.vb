Option Infer On

Imports System
Imports SautinSoft.Pdf
Imports System.IO
Imports SautinSoft.Pdf.Content

Friend Class Program
	''' <summary>
	''' How to set Font's settings and text formatting.
	''' </summary>
	''' <remarks>
	''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/text-from-complex-scripts.php
	''' </remarks>
	Shared Sub Main()
		' Before starting this example, please get a free license:
		' https://sautinsoft.com/start-for-free/

		' Apply the key here:
		' PdfDocument.SetLicense("...");

		Using document = New PdfDocument()
			' Create a new page.
			Dim page = document.Pages.Add()

			Using formattedText = New PdfFormattedText()
				' Set up and fill a PdfFormattedText object with multilingual text.
				formattedText.Language = New PdfLanguage("en-US")
				formattedText.Font = New PdfFont("Calibri", 12)

				formattedText.AppendLine("An example of a fully vocalised (vowelised or vowelled) Arabic ").Append("from the Basmala: ")

				formattedText.Language = New PdfLanguage("ar-SA")
				formattedText.Font = New PdfFont("Arial", 24)
				formattedText.Append("بِسْمِ ٱللَّٰهِ ٱلرَّحْمَٰنِ ٱلرَّحِيمِ")

				formattedText.Language = New PdfLanguage("en-US")
				formattedText.Font = New PdfFont("Calibri", 12)
				formattedText.AppendLine(", which means: ").Append("In the name of God, the All-Merciful, the Especially-Merciful.")

				' Draw this text.
				page.Content.DrawText(formattedText, New PdfPoint(50, 750))
				' Clear PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill a PdfFormattedText object with multilingual text.
				formattedText.Append("An example of Hebrew: ")

				formattedText.Language = New PdfLanguage("he-IL")
				formattedText.Font = New PdfFont("Arial", 24)
				formattedText.Append("מה קורה")

				formattedText.Language = New PdfLanguage("en-US")
				formattedText.Font = New PdfFont("Calibri", 12)
				formattedText.AppendLine(", which means: What's going on, ").Append("and ")

				formattedText.Language = New PdfLanguage("he-IL")
				formattedText.Font = New PdfFont("Arial", 24)
				formattedText.Append("תודה לכולם")

				formattedText.Language = New PdfLanguage("en-US")
				formattedText.Font = New PdfFont("Calibri", 12)
				formattedText.Append(", which means: Thank you all.")
				' Draw this text.
				page.Content.DrawText(formattedText, New PdfPoint(50, 650))
				' Clear PdfFormattedText object.
				formattedText.Clear()
				' Set up and fill a PdfFormattedText object with multilingual text.
				formattedText.LineHeight = 50

				formattedText.Append("An example of Thai: ")
				formattedText.Language = New PdfLanguage("th-TH")
				formattedText.Font = New PdfFont("Leelawadee UI", 16)
				formattedText.AppendLine("ภัำ")

				formattedText.Language = New PdfLanguage("en-US")
				formattedText.Font = New PdfFont("Calibri", 12)
				formattedText.Append("An example of Tamil: ")
				formattedText.Language = New PdfLanguage("ta-IN")
				formattedText.Font = New PdfFont("Nirmala UI", 16)
				formattedText.AppendLine("போது")

				formattedText.Language = New PdfLanguage("en-US")
				formattedText.Font = New PdfFont("Calibri", 12)
				formattedText.Append("An example of Bengali: ")
				formattedText.Language = New PdfLanguage("be-IN")
				formattedText.Font = New PdfFont("Nirmala UI", 16)
				formattedText.AppendLine("আবেদনকারীর মাতার পিতার বর্তমান স্থায়ী ঠিকানা নমিনি নাম")

				formattedText.Language = New PdfLanguage("en-US")
				formattedText.Font = New PdfFont("Calibri", 12)
				formattedText.Append("An example of Gujarati: ")
				formattedText.Language = New PdfLanguage("gu-IN")
				formattedText.Font = New PdfFont("Nirmala UI", 16)
				formattedText.AppendLine("કાર્બન કેમેસ્ટ્રી")

				formattedText.Language = New PdfLanguage("en-US")
				formattedText.Font = New PdfFont("Calibri", 12)
				formattedText.Append("An example of Osage: ")
				formattedText.Language = New PdfLanguage("osa")
				formattedText.Font = New PdfFont("Gadugi", 16)
				formattedText.Append("𐓏𐓘𐓻𐓘𐓻𐓟 𐒻𐓟")
				' Draw this text.
				page.Content.DrawText(formattedText, New PdfPoint(50, 350))
			End Using
			' Save PDF Document.
			document.Save("Complex scripts.pdf")
			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Complex scripts.pdf") With {.UseShellExecute = True})
		End Using
	End Sub
End Class
