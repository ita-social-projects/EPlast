using EPlast.BLL.DTO;
using EPlast.BLL.ExtensionMethods;
using EPlast.DataAccess.Entities;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;

namespace EPlast.BLL.Services.PDF.Documents
{
    public class MethodicDocumentPdf : PdfDocument
    {
        private readonly MethodicDocument _methodicDocument;
        private const int TextWidth = 510;
        private const int LeftIndent = 60;
        private const int BottomRectHeightWithStock = 70;
        private const int BottomRectHeight = 50;
        private const string FontName = "Times New Roman";
        private const int BaseFontSize = 14;
        private const int TextFontSize = 12;

        public MethodicDocumentPdf(MethodicDocument methodicDocument) : this(methodicDocument, new PdfSettings())
        {
        }

        public MethodicDocumentPdf(MethodicDocument methodicDocument, IPdfSettings settings) : base(settings)
        {
            _methodicDocument = methodicDocument;
        }

        public override void SetDocumentBody(PdfPage page, XGraphics gfx)
        {
            DrawText(gfx, page);
        }

        protected void DrawText(XGraphics gfx, PdfPage page)
        {
            const int rectHeight = 200;
            const int middleRectY = 300;
            const int topRectY = 230;
            const int topRectHeight = 300;
            var options = new XPdfFontOptions(PdfFontEncoding.Unicode);
            var font = new XFont(FontName, BaseFontSize, XFontStyle.Regular, options);

            var format = new XStringFormat();

            var topRect = new XRect(LeftIndent, topRectY, TextWidth, topRectHeight);
            gfx.DrawRectangle(XBrushes.White, topRect);

            format.Alignment = XStringAlignment.Far;

            gfx.DrawString($"{_methodicDocument.Name} від {_methodicDocument.Date:dd/MM/yyyy}", font, XBrushes.Black, topRect, format);

            font = new XFont(FontName, TextFontSize, XFontStyle.Regular, options);

            var tfx = new TextFormatter(gfx);

            var middleRect = new XRect(LeftIndent, middleRectY, TextWidth, rectHeight);
            tfx.PrepareDrawString(_methodicDocument.Description, font, new XRect(LeftIndent, middleRectY, TextWidth, double.MaxValue),
                out var lastCharIndex, out var neededHeight);
            if (neededHeight > rectHeight)
            {
                middleRect = new XRect(LeftIndent, middleRectY, TextWidth, neededHeight);
            }

            if (neededHeight > page.Height - rectHeight - middleRect.Y)
            {
                middleRect = new XRect(LeftIndent, middleRectY, TextWidth, page.Height - middleRect.Y - 30);
                tfx.PrepareDrawString(_methodicDocument.Description, font, middleRect,
                    out lastCharIndex, out neededHeight);
                middleRect = new XRect(LeftIndent, middleRectY, TextWidth, neededHeight);
            }

            tfx.DrawString(_methodicDocument.Description, font, XBrushes.Black, middleRect, XStringFormats.TopLeft);
            if (lastCharIndex == -1)
            {
                if (page.Height < neededHeight + middleRectY + BottomRectHeightWithStock)
                {
                    var newPage = document.AddPage();
                    gfx = XGraphics.FromPdfPage(newPage);
                    middleRect = new XRect(LeftIndent, 0, TextWidth, 0);
                }
                DrawBottom(gfx, format, middleRect, font);
            }
            else
            {
                DrawNextPage(_methodicDocument.Description[(lastCharIndex + 1)..], font);
            }
        }

        private void DrawNextPage(string text, XFont font)
        {
            while (true)
            {
                const int middleRectY = 40;
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var format = new XStringFormat();
                var tfx = new TextFormatter(gfx);
                var middleRect = new XRect(LeftIndent, middleRectY, TextWidth, double.MaxValue);
                tfx.PrepareDrawString(text, font, middleRect, out var newIndexOfLastCharOnPage, out var newNeededHeight);
                if (newNeededHeight > page.Height - BottomRectHeight)
                {
                    middleRect = new XRect(LeftIndent, middleRectY, TextWidth, page.Height - BottomRectHeight);
                    tfx.PrepareDrawString(text, font, middleRect, out newIndexOfLastCharOnPage, out newNeededHeight);
                }

                middleRect = new XRect(LeftIndent, middleRectY, TextWidth, newNeededHeight);

                tfx.DrawString(text, font, XBrushes.Black, middleRect, format);

                if (newIndexOfLastCharOnPage == -1)
                {
                    if (page.Height < newNeededHeight + middleRectY + BottomRectHeightWithStock)
                    {
                        var newPage = document.AddPage();
                        gfx = XGraphics.FromPdfPage(newPage);
                        middleRect = new XRect(LeftIndent, 0, TextWidth, 0);
                    }

                    DrawBottom(gfx, format, middleRect, font);
                }
                else
                {
                    text = text[(newIndexOfLastCharOnPage + 1)..];
                    continue;
                }

                break;
            }
        }

        private void DrawBottom(XGraphics gfx, XStringFormat format, XRect middleRect, XFont font)
        {
            var bottomRect = new XRect(LeftIndent, 10 + middleRect.Y + middleRect.Height, TextWidth, BottomRectHeight);
            format.Alignment = XStringAlignment.Far;
            format.LineAlignment = XLineAlignment.Far;
            font = new XFont(font.Name, BaseFontSize, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
            gfx.DrawString($"Тип документу: {MethodicDocumentTypeParser(_methodicDocument.Type)}", font, XBrushes.Black,
                bottomRect, format);
        }

        private string MethodicDocumentTypeParser(string type)
        {
            return type switch
            {
                "legislation" => MethodicDocumentTypeDTO.legislation.GetDescription(),
                "Methodics" => MethodicDocumentTypeDTO.Methodics.GetDescription(),
                "Other" => MethodicDocumentTypeDTO.Other.GetDescription(),
                _ => "Не визначено"
            };
        }
    }
}
