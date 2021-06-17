using EPlast.BLL.ExtensionMethods;
using EPlast.DataAccess.Entities;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;

namespace EPlast.BLL.Services.PDF.Documents
{
    public class DecisionDocument : PdfDocument
    {
        private readonly Decesion _decision;
        private const int TextWidth = 510;
        private const int LeftIndent = 60;
        private const int BottomRectHeightWithStock = 70;
        private const int BottomRectHeight = 50;
        private const string FontName = "Times New Roman";
        private const int BaseFontSize = 14;
        private const int TextFontSize = 12;

        public DecisionDocument(Decesion decision) : this(decision, new PdfSettings())
        {
        }

        public DecisionDocument(Decesion decision, IPdfSettings settings) : base(settings)
        {
            this._decision = decision;
        }

        public override void SetDocumentBody(PdfPage page, XGraphics gfx)
        {
            DrawText(gfx, page);
        }

        protected void DrawText(XGraphics gfx, PdfPage page)
        {
            int rectHeight = 200;
            int middleRectY = 300;
            int topRectY = 230;
            int topRectHeight = 300;
            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);
            XFont font = new XFont(FontName, BaseFontSize, XFontStyle.Regular, options);

            XStringFormat format = new XStringFormat();


            XRect topRect = new XRect(LeftIndent, topRectY, TextWidth, topRectHeight);
            gfx.DrawRectangle(XBrushes.White, topRect);

            format.Alignment = XStringAlignment.Far;

            gfx.DrawString($"{_decision.Name} від {_decision.Date:dd/MM/yyyy}", font, XBrushes.Black, topRect, format);

            font = new XFont(FontName, TextFontSize, XFontStyle.Regular, options);

            TextFormatter tfx = new TextFormatter(gfx);

            XRect middleRect = new XRect(LeftIndent, middleRectY, TextWidth, rectHeight);
            tfx.PrepareDrawString(_decision.Description, font, new XRect(LeftIndent, middleRectY, TextWidth, double.MaxValue),
                out var lastCharIndex, out var neededHeight);
            if (neededHeight > rectHeight)
            {
                middleRect = new XRect(LeftIndent, middleRectY, TextWidth, neededHeight);
            }

            if (neededHeight > page.Height - rectHeight - middleRect.Y)
            {
                middleRect = new XRect(LeftIndent, middleRectY, TextWidth, page.Height - middleRect.Y - 30);
                tfx.PrepareDrawString(_decision.Description, font, middleRect,
                    out lastCharIndex, out neededHeight);
                middleRect = new XRect(LeftIndent, middleRectY, TextWidth, neededHeight);
            }

            tfx.DrawString(_decision.Description, font, XBrushes.Black, middleRect, XStringFormats.TopLeft);
            if (lastCharIndex == -1)
            {
                if (page.Height<neededHeight + middleRectY + BottomRectHeightWithStock)
                {
                    PdfPage newPage = document.AddPage();
                    gfx = XGraphics.FromPdfPage(newPage);
                    middleRect = new XRect(LeftIndent, 0, TextWidth, 0);
                }
                DrawBottom(gfx, format, middleRect, font);
                
            }
            else
            {
                DrawNextPage(_decision.Description.Substring(lastCharIndex + 1), font);
            }
        }

        void DrawNextPage(string text, XFont font)
        {
            int middleRectY = 40;
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XStringFormat format = new XStringFormat();
            TextFormatter tfx = new TextFormatter(gfx);
            XRect middleRect = new XRect(LeftIndent, middleRectY, TextWidth, double.MaxValue);
            tfx.PrepareDrawString(text, font, middleRect,
                out var newIndexOfLastCharOnPage, out var newNeededHeight);
            if (newNeededHeight > page.Height - BottomRectHeight)
            {
                middleRect = new XRect(LeftIndent, middleRectY, TextWidth, page.Height - BottomRectHeight);
                tfx.PrepareDrawString(text, font, middleRect,
                    out newIndexOfLastCharOnPage, out newNeededHeight);
            }

            middleRect = new XRect(LeftIndent, middleRectY, TextWidth, newNeededHeight);


            tfx.DrawString(text, font, XBrushes.Black, middleRect, format);


            if (newIndexOfLastCharOnPage == -1)
            {
                if (page.Height < newNeededHeight + middleRectY + BottomRectHeightWithStock)
                {
                    PdfPage newPage = document.AddPage();
                    gfx = XGraphics.FromPdfPage(newPage);
                    middleRect = new XRect(LeftIndent, 0, TextWidth, 0);
                }
                DrawBottom(gfx, format, middleRect, font);
                
            }
            else
            {
                DrawNextPage(text.Substring(newIndexOfLastCharOnPage + 1), font);
            }
        }

        private void DrawBottom(XGraphics gfx, XStringFormat format, XRect middleRect, XFont font)
        {
            XRect bottomRect = new XRect(LeftIndent, 10 + middleRect.Y + middleRect.Height, TextWidth, BottomRectHeight);
            format.Alignment = XStringAlignment.Far;
            format.LineAlignment = XLineAlignment.Far;
            font = new XFont(font.Name, BaseFontSize, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
            gfx.DrawString($"Поточний статус: {_decision.DecesionStatusType.GetDescription()}", font, XBrushes.Black,
                bottomRect, format);
        }
    }
}
