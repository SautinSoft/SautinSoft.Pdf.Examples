Option Infer On

Imports System
Imports System.IO
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Forms

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Fill in PDF interactive forms.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/flatten-interactive-forms.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free 100-day trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\FormFilled.pdf")

			Using document = PdfDocument.Load(pdfFile)
				' A flag specifying whether to construct appearance for all form fields in the document.
				Dim needAppearances As Boolean = document.Form.NeedAppearances

				For Each field In document.Form.Fields
					' Do not flatten button fields.
					If field.FieldType = PdfFieldType.Button Then
						Continue For
					End If

					' Construct appearance, if needed.
					If needAppearances Then
						field.Appearance.Refresh()
					End If

					' Get the field's appearance form.
					Dim fieldAppearance = field.Appearance.Get()

					' If the field doesn't have an appearance, skip it.
					If fieldAppearance Is Nothing Then
						Continue For
					End If

					' Add a new content group to the field's page and
					' add new form content with the field's appearance form to the content group.
					' The content group is added so that transformation from the next statement is localized to the content group.
					Dim flattenedContent = field.Page.Content.Elements.AddGroup().Elements.AddForm(fieldAppearance)

					' Translate the form content to the same position on the page that the field is in.
					Dim fieldBounds = field.Bounds
					flattenedContent.Transform = PdfMatrix.CreateTranslation(fieldBounds.Left, fieldBounds.Bottom)
				Next field

				' Remove all fields, thus making the document non-interactive,
				' since their appearance is now contained directly in the content of their pages.
				document.Form.Fields.Clear()

				document.Save("FormFlattened.pdf")
			End Using

			System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("FormFlattened.pdf") With {.UseShellExecute = True})
		End Sub
	End Class
End Namespace
