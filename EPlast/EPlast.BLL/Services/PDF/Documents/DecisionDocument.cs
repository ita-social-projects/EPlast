﻿using System.Collections.Generic;
using EPlast.BLL.ExtensionMethods;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;

namespace EPlast.BLL
{
    public class DecisionDocument : PdfDocument
    {
        private readonly Decesion _decision;
        private const int TextWidth = 510;
        private const int LeftIndent = 60;
        private const string FontName = "Times New Roman";

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
            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);
            XFont font = new XFont(FontName, 14, XFontStyle.Regular, options);

            XStringFormat format = new XStringFormat();


            XRect topRect = new XRect(LeftIndent, 230, TextWidth, 300);
            gfx.DrawRectangle(XBrushes.White, topRect);

            format.Alignment = XStringAlignment.Far;

            gfx.DrawString($"{_decision.Name} від {_decision.Date:dd/MM/yyyy}", font, XBrushes.Black, topRect, format);

            font = new XFont(FontName, 12, XFontStyle.Regular, options);

            TextFormatter tfx = new TextFormatter(gfx);

            XRect middleRect = new XRect(LeftIndent, 300, TextWidth, rectHeight);
            tfx.PrepareDrawString(_decision.Description, font, new XRect(60, 300, TextWidth, double.MaxValue),
                out var lastCharIndex, out var neededHeight);
            if (neededHeight > rectHeight)
            {
                middleRect = new XRect(LeftIndent, 300, TextWidth, neededHeight);
            }

            if (neededHeight > page.Height - rectHeight - middleRect.Y)
            {
                middleRect = new XRect(LeftIndent, 300, TextWidth, page.Height - middleRect.Y - 30);
                tfx.PrepareDrawString(_decision.Description, font, middleRect,
                    out lastCharIndex, out neededHeight);
                middleRect = new XRect(LeftIndent, 300, TextWidth, neededHeight);
            }

            tfx.DrawString(_decision.Description, font, XBrushes.Black, middleRect, XStringFormats.TopLeft);
            if (lastCharIndex == -1)
            {
                DrawBottom(gfx, format, middleRect, font);
            }
            else
            {
                PdfPage newPage = document.AddPage();
                DrawNextPage(newPage, _decision.Description.Substring(lastCharIndex + 1), font);
            }
        }

        void DrawNextPage(PdfPage page, string text, XFont font)
        {
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XStringFormat format = new XStringFormat();
            TextFormatter tfx = new TextFormatter(gfx);
            XRect middleRect = new XRect(LeftIndent, 40, TextWidth, double.MaxValue);
            tfx.PrepareDrawString(text, font, middleRect,
                out var newIndexOfLastCharOnPage, out var newNeededHeight);
            if (newNeededHeight > page.Height - 50)
            {
                middleRect = new XRect(LeftIndent, 40, TextWidth, page.Height - 50);
                tfx.PrepareDrawString(text, font, middleRect,
                    out newIndexOfLastCharOnPage, out newNeededHeight);
            }

            middleRect = new XRect(LeftIndent, 40, TextWidth, newNeededHeight);

            tfx.DrawString(text, font, XBrushes.Black, middleRect, format);


            if (newIndexOfLastCharOnPage == -1)
            {
                DrawBottom(gfx, format, middleRect, font);
            }
            else
            {
                PdfPage newPage = document.AddPage();
                DrawNextPage(newPage, text.Substring(newIndexOfLastCharOnPage + 1), font);
            }
        }

        private void DrawBottom(XGraphics gfx, XStringFormat format, XRect middleRect, XFont font)
        {
            XRect bottomRect = new XRect(LeftIndent, 10 + middleRect.Y + middleRect.Height, TextWidth, 50);
            format.Alignment = XStringAlignment.Far;
            format.LineAlignment = XLineAlignment.Far;
            font = new XFont(font.Name, 14, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
            gfx.DrawString($"Поточний статус: {_decision.DecesionStatusType.GetDescription()}", font, XBrushes.Black,
                bottomRect, format);
        }
    }
}