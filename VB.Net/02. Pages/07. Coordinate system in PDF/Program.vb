Option Infer On
Imports System
Imports System.Drawing
Imports System.IO
Imports System.Reflection.Metadata
Imports System.Security.Cryptography.X509Certificates
Imports SautinSoft
Imports SautinSoft.Pdf
Imports SautinSoft.Pdf.Content

Namespace Sample
    Class Sample
		''' <summary>
        ''' Coordinate system in PDF document using C# and .NET
        ''' </summary>
        ''' <remarks>
        ''' Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/coordinate-system-in-pdf-document-using-csharp-and-dotnet.php
        ''' </remarks>
        Shared Sub Main(ByVal args As String())
			' Before starting this example, please get a free trial key:
            ' https://sautinsoft.com/start-for-free/

            ' Apply the key here:
            ' PdfDocument.SetLicense("...");
			
            Using document = New PdfDocument()
				' Add a page.
                Dim page = document.Pages.Add()
				
				' NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
                ' and the positive y axis extends vertically upward.
                Dim pageBounds = page.CropBox
				
				' Add a thick red line at the top of the page.
                Dim line1 = page.Content.Elements.AddPath()
                line1.BeginSubpath(New PdfPoint(596, pageBounds.Top - 0)).LineTo(New PdfPoint(pageBounds.Left - 0, pageBounds.Top - 0))
                Dim line1Format = line1.Format
                line1Format.Stroke.IsApplied = True
                line1Format.Stroke.Width = 5
                line1Format.Stroke.Color = PdfColor.FromRgb(1, 0, 0)
				
				' Add a thick blue line to the left of the page.
                Dim line2 = page.Content.Elements.AddPath()
                line2.BeginSubpath(New PdfPoint(596, pageBounds.Left - 0)).LineTo(New PdfPoint(pageBounds.Right - 0, pageBounds.Top - 0))
                Dim line2Format = line2.Format
                line2Format.Stroke.IsApplied = True
                line2Format.Stroke.Width = 5
                line2Format.Stroke.Color = PdfColor.FromRgb(0, 0, 1)
				
				' Add a thick red line at the bottom of the page.
                Dim line3 = page.Content.Elements.AddPath()
                line3.BeginSubpath(New PdfPoint(596, pageBounds.Left - 0)).LineTo(New PdfPoint(pageBounds.Left - 0, pageBounds.Bottom - 0))
                Dim line3Format = line3.Format
                line3Format.Stroke.IsApplied = True
                line3Format.Stroke.Width = 5
                line3Format.Stroke.Color = PdfColor.FromRgb(1, 0, 0)
				
				' Add a thick red line to the right of the page.
                Dim line4 = page.Content.Elements.AddPath()
                line4.BeginSubpath(New PdfPoint(0, pageBounds.Right - 596)).LineTo(New PdfPoint(pageBounds.Right - 596, pageBounds.Top - 0))
                Dim line4Format = line4.Format
                line4Format.Stroke.IsApplied = True
                line4Format.Stroke.Width = 5
                line4Format.Stroke.Color = PdfColor.FromRgb(0, 0, 1)
                Dim margin As Double = 15

                Using formattedText = New PdfFormattedText()
					' Set up and fill the PdfFormattedText object.
                    formattedText.TextAlignment = PdfTextAlignment.Left
                    formattedText.MaxTextWidth = 100
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0)
                    formattedText.Append("(")
                    formattedText.Color = PdfColor.FromRgb(1, 0, 0)
                    formattedText.Append("0")
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0)
                    formattedText.Append(";")
                    formattedText.Color = PdfColor.FromRgb(0, 0, 1)
                    formattedText.Append("842")
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0)
                    formattedText.Append(")")
					
					' Draw text in the top-left corner of the page.
                    page.Content.DrawText(formattedText, New PdfPoint(margin, page.CropBox.Top - margin - formattedText.Height))
					
					' Clear the PdfFormattedText object.
                    formattedText.Clear()
					
					' Set up and fill the PdfFormattedText object.
                    formattedText.TextAlignment = PdfTextAlignment.Right
                    formattedText.MaxTextWidth = 100
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0)
                    formattedText.Append("(")
                    formattedText.Color = PdfColor.FromRgb(1, 0, 0)
                    formattedText.Append("596")
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0)
                    formattedText.Append(";")
                    formattedText.Color = PdfColor.FromRgb(0, 0, 1)
                    formattedText.Append("842")
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0)
                    formattedText.Append(")")
					
					' Draw text in the top-right corner of the page.
                    page.Content.DrawText(formattedText, New PdfPoint(page.CropBox.Width - margin - formattedText.MaxTextWidth, page.CropBox.Top - margin - formattedText.Height))
                    
					' Clear the PdfFormattedText object.
					formattedText.Clear()
					
					' Set up and fill the PdfFormattedText object.
                    formattedText.TextAlignment = PdfTextAlignment.Right
                    formattedText.MaxTextWidth = 100
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0)
                    formattedText.Append("(")
                    formattedText.Color = PdfColor.FromRgb(1, 0, 0)
                    formattedText.Append("596")
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0)
                    formattedText.Append(";")
                    formattedText.Color = PdfColor.FromRgb(0, 0, 1)
                    formattedText.Append("0")
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0)
                    formattedText.Append(")")
					
					' Draw text in the bottom-right corner of the page.
                    page.Content.DrawText(formattedText, New PdfPoint(page.CropBox.Width - margin - formattedText.MaxTextWidth, margin))
					
					' Clear the PdfFormattedText object.
                    formattedText.Clear()
					
					' Set up and fill the PdfFormattedText object.
                    formattedText.TextAlignment = PdfTextAlignment.Left
                    formattedText.MaxTextWidth = 100
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0)
                    formattedText.Append("(")
                    formattedText.Color = PdfColor.FromRgb(1, 0, 0)
                    formattedText.Append("0")
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0)
                    formattedText.Append(";")
                    formattedText.Color = PdfColor.FromRgb(0, 0, 1)
                    formattedText.Append("0")
                    formattedText.Color = PdfColor.FromRgb(0, 0, 0)
                    formattedText.Append(")")
					
					' Draw text in the bottom-left corner of the page.
                    page.Content.DrawText(formattedText, New PdfPoint(margin, margin))
					
					' Save a PDF Document.
                    document.Save("Coordinate system.pdf")
                End Using
            End Using

            System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("Coordinate system.pdf") With {
                .UseShellExecute = True
            })
        End Sub
    End Class
End Namespace
