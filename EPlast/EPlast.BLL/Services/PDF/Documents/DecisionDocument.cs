using System.Collections.Generic;
using EPlast.BLL.ExtensionMethods;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            //List<(PdfPage,XGraphics)> pages = new List<(PdfPage, XGraphics)>();
     
            //double totalHeight = gfx.PageSize.Height;
            //if (innerRectHeight > totalHeight)
            //    page.Height = innerRectHeight;
            //while (innerRectHeight > totalHeight)
            //{
            //    PdfPage newPage = document.AddPage();
            //    totalHeight += gfx.PageSize.Height;
            //    XGraphics newGfx = XGraphics.FromPdfPage(newPage);
            //    pages.Add((newPage, newGfx));
            //}
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
            int rectHeight = 200;
            XRect rect2 = new XRect(60, 300, 510, rectHeight);
            //tf.Alignment = ParagraphAlignment.Left;
            tfx.PrepareDrawString(decesion.Description, font, new XRect(60, 300, 510, double.MaxValue), 
                out lastCharIndex, out neededHeight);
            //rect2 = new XRect(60, 300, 510, neededHeight);
            if (neededHeight> rectHeight)
            {
                rect2 = new XRect(60, 300, 510, neededHeight);
            }
            if (neededHeight >page.Height- rectHeight-rect2.Y)
            {
                rect2 = new XRect(60, 300, 510, page.Height /*- rectHeight*/ - rect2.Y-30);
                tfx.PrepareDrawString(decesion.Description, font, rect2,
                    out lastCharIndex, out neededHeight);
                rect2 = new XRect(60, 300, 510, neededHeight);
            }
            //rect2 = new XRect(60, 300, 510, neededHeight);
            //gfx.DrawRectangle(XBrushes.SeaShell, rect);
            tfx.DrawString(decesion.Description, font, XBrushes.Black, rect2, XStringFormats.TopLeft);
            if (lastCharIndex == -1)
            {
                XRect bottomRect = new XRect(60, 10 + rect2.Y+ rect2.Height, 510, 50);
                format.Alignment = XStringAlignment.Far;
                format.LineAlignment = XLineAlignment.Far;
                font = new XFont(fontName, 14, XFontStyle.Regular, options);
                gfx.DrawString($"Поточний статус: {decesion.DecesionStatusType.GetDescription()}", font, XBrushes.Black,
                    bottomRect, format);
            }
            else
            {

                PdfPage newPage = document.AddPage();
                DrawNextPage(newPage, decesion.Description.Substring(lastCharIndex+1), font);
            }
            

        }

        void DrawNextPage(PdfPage page, string text, XFont font)
        {
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XStringFormat format = new XStringFormat();
            XTextFormatterEx2 tfx = new XTextFormatterEx2(gfx);
            double newNeedHeight;
            int newIndexOfLastCharOnPage;
            var rect = new XRect(60, 40, 510, double.MaxValue);
            tfx.PrepareDrawString(text, font, rect,
                out newIndexOfLastCharOnPage, out newNeedHeight);
            if (newNeedHeight > page.Height - 50)
            {
                rect = new XRect(60, 40, 510, page.Height - 50);
                tfx.PrepareDrawString(text, font, rect,
                    out newIndexOfLastCharOnPage, out newNeedHeight);
            }
            rect = new XRect(60, 40, 510, newNeedHeight);

            tfx.DrawString(text,font, XBrushes.Black,rect,format);
         


            if (newIndexOfLastCharOnPage == -1)
            {
                XRect bottomRect = new XRect(60, 10+newNeedHeight, 510, 50);
                format.Alignment = XStringAlignment.Far;
                format.LineAlignment = XLineAlignment.Far;
                font = new XFont(font.Name, 14, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
                
                gfx.DrawString($"Поточний статус: {decesion.DecesionStatusType.GetDescription()}", font, XBrushes.Black,
                    bottomRect, format);
                return;
            }
            PdfPage newPage = document.AddPage();
            DrawNextPage(newPage, text.Substring(newIndexOfLastCharOnPage+1),font);
        }
    }
}