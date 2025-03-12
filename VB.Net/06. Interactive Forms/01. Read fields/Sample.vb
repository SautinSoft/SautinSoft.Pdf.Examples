Option Infer On

Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content
Imports SautinSoft.Pdf.Forms

Namespace Sample
	Friend Class Sample
		''' <summary>
		''' Read PDF interactive form fields.
		''' </summary>
		''' <remarks>
		''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/read-interactive-form.php
		''' </remarks>
		Shared Sub Main(ByVal args() As String)
			' Before starting this example, please get a free trial key:
			' https://sautinsoft.com/start-for-free/

			' Apply the key here:
			' PdfDocument.SetLicense("...");

			Dim pdfFile As String = Path.GetFullPath("..\..\..\FormFilled.pdf")

			Dim writer = New StringWriter(CultureInfo.InvariantCulture)
			Dim format As String = "{0,-16}|{1,20}|{2,-20}|{3,-20}|{4,-20}", separator As New String("-"c, 100)

			' Write the header.
			writer.WriteLine("Document contains the following form fields:")
			writer.WriteLine()
			writer.WriteLine(format, "Type", """"c & "Name" & """"c, "Value", "ExportValue/Choice", "Checked/Selected")
			writer.WriteLine(separator)

			Dim fieldType? As PdfFieldType
			Dim fieldName, fieldExportValueOrChoice As String
			Dim fieldValue As Object
			Dim fieldCheckedOrSelected? As Boolean

			Using document = PdfDocument.Load(pdfFile)
				' Group fields by name because all fields with the same name are actually different representations (widget annotations) of the same field.
				' Radio button fields are usually grouped. Other field types are rarely grouped.
				For Each fieldGroup In document.Form.Fields.GroupBy(Function(field) field.Name)
					Dim field = fieldGroup.First()

					fieldType = field.FieldType
					fieldName = """"c & field.Name & """"c
					fieldValue = field.Value

					For Each widgetField In fieldGroup
						Select Case widgetField.FieldType
							Case PdfFieldType.CheckBox, PdfFieldType.RadioButton
								' Check box and radio button are toggle button fields.
								Dim toggleField = CType(widgetField, PdfToggleButtonField)

								fieldExportValueOrChoice = If(toggleField.FieldType = PdfFieldType.CheckBox, CType(toggleField, PdfCheckBoxField).ExportValue, CType(toggleField, PdfRadioButtonField).Choice)
								fieldCheckedOrSelected = toggleField.Checked

								writer.WriteLine(format, fieldType, fieldName, fieldValue, fieldExportValueOrChoice, fieldCheckedOrSelected)

							Case PdfFieldType.ListBox, PdfFieldType.Dropdown
								' List box and drop-down are choice fields.
								Dim choiceField = CType(widgetField, PdfChoiceField)

								' List box can have multiple values if multiple selection is enabled.
								Dim tempVar As Boolean = TypeOf fieldValue Is String()
								Dim fieldValues() As String = If(tempVar, DirectCast(fieldValue, String()), Nothing)
								If tempVar Then
									fieldValue = String.Join(", ", fieldValues)
								End If

								For itemIndex As Integer = 0 To choiceField.Items.Count - 1
									fieldExportValueOrChoice = If(choiceField.Items(itemIndex).ExportValue, choiceField.Items(itemIndex).Value)
									fieldCheckedOrSelected = If(choiceField.FieldType = PdfFieldType.ListBox, CType(choiceField, PdfListBoxField).SelectedIndices.Contains(itemIndex), CType(choiceField, PdfDropdownField).SelectedIndex = itemIndex)

									writer.WriteLine(format, fieldType, fieldName, fieldValue, fieldExportValueOrChoice, fieldCheckedOrSelected)

									' Write the field type, field name, and field value just once for a field group.
									fieldType = Nothing
									fieldName = Nothing
									fieldValue = Nothing
								Next itemIndex

							Case Else
								' Text field may contain multiple lines of text, if enabled.
								If widgetField.FieldType = PdfFieldType.Text AndAlso CType(widgetField, PdfTextField).MultiLine AndAlso fieldValue IsNot Nothing Then
									fieldValue = DirectCast(fieldValue, String).Replace(vbCr, "\r")
								End If

								fieldExportValueOrChoice = Nothing
								fieldCheckedOrSelected = Nothing

								writer.WriteLine(format, fieldType, fieldName, fieldValue, fieldExportValueOrChoice, fieldCheckedOrSelected)
						End Select

						' Write the field type, field name, and field value just once for a field group.
						fieldType = Nothing
						fieldName = Nothing
						fieldValue = Nothing
					Next widgetField

					writer.WriteLine(separator)
				Next fieldGroup
			End Using

			Console.Write(writer.ToString())
		End Sub
	End Class
End Namespace
