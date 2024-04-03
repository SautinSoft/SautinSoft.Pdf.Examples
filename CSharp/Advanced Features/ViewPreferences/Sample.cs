using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        static void Main(string[] args)
        {
            string pdfFile = Path.GetFullPath(@"..\..\..\simple text.pdf");

            using (var document = PdfDocument.Load(pdfFile))
            {
                // Get viewer preferences specifying the way the document should be displayed on the screen.
                var viewerPreferences = document.ViewerPreferences;

                // Modify viewer preferences.
                viewerPreferences.CenterWindow = false;
                viewerPreferences.FitWindow = true;
                viewerPreferences.HideMenubar = true;
                viewerPreferences.HideToolbar = true;
                viewerPreferences.NonFullScreenPageMode = PdfPageMode.FullScreen;
                viewerPreferences.ViewArea = PdfPageBoundaryType.MediaBox;

                document.Save("Viewer Preferences.pdf");
            }

        }
    }
}
