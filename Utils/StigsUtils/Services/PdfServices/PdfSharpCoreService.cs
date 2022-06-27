// Copyright © 2021 Stig Schmidt Nielsson. This file is open source and distributed under the MIT License, see LICENSE file.

namespace StigsUtils.Common.Services.PdfServices {
	/// <summary>
	/// IPdfService implementation which uses PdfSharpCore service from the CsvHelper package.
	/// </summary>
	public class PdfSharpCoreService : IPdfService {
		
		// Sample code uses <PackageReference Include="PdfSharpCore" Version="1.2.16" />
		
		//Snippets for future inspiration:
		
		/*
		 
		 // how to open existing PDF (represented by array of bytes) to add content:
		using var memoryStream = new MemoryStream(templateBytes);
		using var pdfDocument = PdfReader.Open(memoryStream);
		var tablePageTemplate = pdfDocument.Pages[2].Clone();
		XGraphics pdf = XGraphics.FromPdfPage(pdfDocument.Pages[0]);

		// fonts and brushes examples...
        private static XBrush TextBrush { get; } = XBrushes.Black;
        private static XFont TextFont { get; } = new XFont("Arial", 9.5, XFontStyle.Regular, XPdfFontOptions.WinAnsiDefault);
        private static XFont TextFontBold { get; } = new XFont("Arial", 9.5, XFontStyle.Bold, XPdfFontOptions.WinAnsiDefault);
        private static XFont HeaderFont { get; } = new XFont("Arial", 9.0, XFontStyle.Bold, XPdfFontOptions.WinAnsiDefault);
        private static XFont RowFont { get; } = new XFont("Arial", 7.0, XFontStyle.Regular, XPdfFontOptions.WinAnsiDefault);
        private static XFont PageNumberFont { get; } = new XFont("Arial", 6.0, XFontStyle.Regular, XPdfFontOptions.WinAnsiDefault);
		
            pdf.DrawString("...", TextFontBold, TextBrush, 334, 160);
            pdf.Dispose();

		// drawing string in with max width and truncation using string measuring....
        private static void DrawStringInTableCell(XGraphics pdf, double x, double y, double width, string? text)
        {
            if (string.IsNullOrEmpty(text)) return;
            var size = pdf.MeasureString(text, RowFont);
            while (size.Width > width - 2)
            {
                if (text.Length == 0) return;
                var charsToCut = Math.Min(1, (int)(size.Width / Math.Max(width - 2, 1)));
                text = text.Substring(0, Math.Max(1, text.Length - charsToCut));
                if (text.Length == 0) return;
                size = pdf.MeasureString(text, RowFont);
            }

            x += (width - size.Width) / 2.0;
            pdf.DrawString(text, RowFont, RowBrush, x, y - RowMarginBottom);
        }
        
        

            
            		
		*/
		
	}
}