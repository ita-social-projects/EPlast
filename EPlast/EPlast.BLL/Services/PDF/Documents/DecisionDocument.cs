using System.Collections.Generic;
using EPlast.BLL.ExtensionMethods;
using EPlast.DataAccess.Entities;
using PdfSharp.Drawing.Layout;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;

namespace EPlast.BLL
{
    public class DecisionDocument : PdfDocument
    {
        private readonly Decesion decesion;

        public DecisionDocument(Decesion decesion) : this(decesion, new PdfSettings())
        {
        }

        public DecisionDocument(Decesion decesion, IPdfSettings settings) : base(settings)
        {
            this.decesion = decesion;
        }

        public override void SetDocumentBody(PdfPage page, XGraphics gfx)
        {
            DrawText(gfx, decesion, page);
        }
        protected void DrawText(XGraphics gfx, Decesion decesion, PdfPage page)
        {
            const string fontName = "Times New Roman";

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);
            XFont font = new XFont(fontName, 14, XFontStyle.Regular, options);

            XStringFormat format = new XStringFormat();
            //string text = decesion.Description;
            //string text = FormatDescription(decesion.Description);
            XTextFormatter tf = new XTextFormatter(gfx);

            XRect rect = new XRect(60, 230, 510, 300);
            gfx.DrawRectangle(XBrushes.White, rect);

            format.Alignment = XStringAlignment.Far;
            //format.LineAlignment = XLineAlignment.Center;
            //text = $"{decesion.Name} від {decesion.Date:dd/MM/yyyy}";
            gfx.DrawString($"{decesion.Name} від {decesion.Date:dd/MM/yyyy}", font, XBrushes.Black, rect, format);

            font = new XFont(fontName, 12, XFontStyle.Regular, options);
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Center;
            double innerRectHeight = gfx.MeasureString(decesion.Description, font).Height *
                                    (gfx.MeasureString(decesion.Description, font).Width / 510);
            XRect innerRect = new XRect(60, 300, 510, innerRectHeight);//250
            List<(PdfPage,XGraphics)> pages = new List<(PdfPage, XGraphics)>();
     
            double totalHeight = gfx.PageSize.Height;
            //if (innerRectHeight > totalHeight)
            //    page.Height = innerRectHeight;
            while (innerRectHeight > totalHeight)
            {
                PdfPage newPage = document.AddPage();
                totalHeight += gfx.PageSize.Height;
                XGraphics newGfx = XGraphics.FromPdfPage(newPage);
                pages.Add((newPage, newGfx));
            }
            //tf.DrawString();
            //gfx.DrawString(decesion.Description, font, XBrushes.Black, rect, format,tf);
            //while (gfx.MeasureString(decesion.Description, font).Height >= innerRect.Height)
            //    innerRect.Height += 50;

            ////tf.DrawString(decesion.Description, font, XBrushes.Black, innerRect, XStringFormats.TopLeft);
            XTextFormatterEx2 tfx = new XTextFormatterEx2(gfx);
            int lastCharIndex;
            double neededHeight;

            // Draw the text in a box with the optimal height
            // (magic: we know that one page is enough).
            XRect rect2 = new XRect(40, 100, 250, double.MaxValue);
            //tf.Alignment = ParagraphAlignment.Left;
            tfx.PrepareDrawString(decesion.Description, font, rect2,
                out lastCharIndex, out neededHeight);
            rect2 = new XRect(60, 300, 510, neededHeight);
            //gfx.DrawRectangle(XBrushes.SeaShell, rect);
            tfx.DrawString(decesion.Description, font, XBrushes.Black, rect2, XStringFormats.TopLeft);

            //gfx.DrawString(text, font, XBrushes.Black, 70, 330, format);
            //format.Alignment = XStringAlignment.Far;
            //text = $"{decesion.Name} від {decesion.Date:dd/MM/yyyy}";
            //font = new XFont(fontName, 14, XFontStyle.Regular, options);
            //gfx.DrawString(text, font, XBrushes.Black, rect, format);

            format.Alignment = XStringAlignment.Far;
            format.LineAlignment = XLineAlignment.Far;
            font = new XFont(fontName, 14, XFontStyle.Regular, options);
            //text = $"Поточний статус: {decesion.DecesionStatusType.GetDescription()}";
            //while (gfx.MeasureString(decesion.Description, font).Height + 300 > innerRect.Height)
            //    innerRect.Height += 50;
            XRect bottomRect = new XRect(60, 530, 510, 50);
            while (tf.LayoutRectangle.IntersectsWith(bottomRect))
                bottomRect.Y +=  50;
            gfx.DrawString($"Поточний статус: {decesion.DecesionStatusType.GetDescription()}", font, XBrushes.Black, rect2, format);

        }
    }
}