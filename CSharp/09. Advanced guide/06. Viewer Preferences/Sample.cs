using System;
using System.IO;
using SautinSoft;
using SautinSoft.Pdf;
using SautinSoft.Pdf.Content;

namespace Sample
{
    class Sample
    {
        /// <summary>
        /// Get and set PDF viewer preferences.
        /// </summary>
        /// <remarks>
        /// Details: https://sautinsoft.com/products/pdf/help/net/developer-guide/viewer-preferences.php
        /// </remarks>
        static void Main(string[] args)
        {
            // Before starting this example, please get a free 30-day trial key:
            // https://sautinsoft.com/start-for-free/

            // Apply the key here:
            // PdfDocument.SetLicense("...");

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

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("Viewer Preferences.pdf") { UseShellExecute = true });
        }
    }
}
