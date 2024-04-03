![Nuget](https://img.shields.io/nuget/v/sautinsoft.pdf) 
![Nuget](https://img.shields.io/nuget/dt/sautinsoft.pdf) 
![GitHub](https://img.shields.io/github/license/SautinSoft/SautinSoft.Pdf.Examples)
# PDF .Net is a .NET component built to allow developers to create PDF documents, whether simple or complex, on the fly programmatically. 
![logo_pdf](https://github.com/SautinSoft/SautinSoft.Pdf.Examples/assets/79837963/c33f0a02-eb2c-4831-b7b1-4e776311f744)

PDF .Net allows developers to insert tables, graphs, images, hyperlinks, custom fonts - and more - into PDF documents. 


## Quick links
===============================
+ [Developer Guide](https://sautinsoft.com/products/pdf/help/net/)
+ [API Reference](https://sautinsoft.com/products/pdf/help/net/api-reference/html/R_Project_Pdf__Net_-_API_Reference.htm)


## Top Features
===============================

+ Merge, split, and create PDF files. 

+ Convert PDF files to image, such as PNG, JPEG, GIF, BMP, TIFF.

+ Extract elements from PDF. 

+ View PDF files in WPF applications. 

+ Annotate PDF pages with hyperlinks. 

+ Fill in, flatten, read, and export interactive forms. 

+ Read, write, and update PDF files. 

+ Get, create, or edit bookmarks (outlines) and document properties. 

+ Extract images from PDF files and OCR text from scanned PDFs.

+ Add text, images, shapes (paths), form XObjects, content groups, marked content, and format the content. 

+ Encrypt and digitally sign PDF files. Clone or import pages between PDF documents.

+ Add watermarks, headers, footers, and viewer preferences to PDF pages. 

+ Get, create, remove, or reorder pages. 

Code samples right here: https://github.com/SautinSoft/SautinSoft.Pdf.Examples


## Supported PDF versions:
===============================
* PDF versions 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, /A, 2.0.

## Fonts:
===============================
* A lot of core fonts.
* Type 1 fonts.	
* TrueType fonts.
* Type 3 fonts.
* CJK fonts.	
* Unicode support

## Platforms:
===============================
* .NET Framework 4.7.2. and higher
* .NET 5 and higher
* .Net Core

## OS Support:
===============================
* All versions of Windows (32-bit and 64-bit).
* All versions of Linux (with .NET inside/ 32-bit and 64-bit).
* Cloud Computing Services: AWS, Azure, Google Cloud, etc.

## Getting Started with PDF .Net
===============================
Are you ready to give PDF .NET a try? Simply execute `Install-Package sautinsoft.pdf` from Package Manager Console in Visual Studio to fetch the NuGet package. If you already have PDF .NET and want to upgrade the version, please execute `Update-Package sautinsoft.pdf` to get the latest version.

## Convert PDF document

```csharp
// Load a PDF document.
using (var document = PdfDocument.Load(@"..\..\..\simple text.pdf"))
       {  
       // Create image save options.
            var imageOptions = new ImageSaveOptions(ImageSaveFormat.Jpeg)
                {    PageIndex = 0,
                    // PageNumber = 0, // Select the first PDF page.
                    Width = 1240 // Set the image width and keep the aspect ratio.
                };
        // Save a PDF document to a JPEG file.
        document.Save("Output.jpg", imageOptions);
```
## PDF. Document properties

```csharp
using (var document = PdfDocument.Load(pdfFile))
            {
                // Get document properties.
                var info = document.Info;

                // Update document properties.
                info.Title = "My Title";
                info.Author = "My Author";
                info.Subject = "My Subject";
                info.Creator = "My Application";

                // Update producer and date information, and disable their overriding.
                info.Producer = "My Producer";
                info.CreationDate = new DateTime(2023, 1, 1, 12, 0, 0);
                info.ModificationDate = new DateTime(2023, 1, 1, 12, 0, 0);
                document.SaveOptions.UpdateProducerInformation = false;
                document.SaveOptions.UpdateDateInformation = false;

                document.Save("Document Properties.pdf");
            }

```

## Read PDF document

```csharp
string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");
            using (var document = PdfDocument.Load(pdfFile))
            {
                foreach (var page in document.Pages)
                {
                    Console.WriteLine(page.Content.ToString());
                }
            }

```
## Resources
===============================
+ **Website:** [www.sautinsoft.com](https://www.sautinsoft.com)
+ **Product Home:** [PDF .Net](https://sautinsoft.com/products/pdf/)
+ [Download PDF .Net](https://sautinsoft.com/products/pdf/download.php)
+ [Developer Guide](https://sautinsoft.com/products/pdf/help/net/)
+ [API Reference](https://sautinsoft.com/products/pdf/help/net/api-reference/html/R_Project_PDF__Net_-_API_Reference.htm)
+ [Support Team](https://sautinsoft.com/support.php)
+ [License](https://sautinsoft.com/products/pdf/help/net/getting-started/agreement.php)

PDF .Net totally simplifies the development of .NET applications where require to manipulate with PDF documents. Let us say, to provide the method to rotate a page in PDF document, you have add only the reference to the "SautinSoft.Pdf.dll" and write 3-4 C# lines in your application. 

If you need any help with code samples, API, prices or another issues, please use our WebChat, Skype, Email, Phones:

+46 855924509 (Support)
+46 812111486 (Sales)
Skype: sautinsoft.support
Email: support@sautinsoft.com

