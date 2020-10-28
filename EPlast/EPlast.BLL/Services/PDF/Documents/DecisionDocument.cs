using EPlast.BLL.ExtensionMethods;
using EPlast.DataAccess.Entities;
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
            DrawText(gfx, decesion);
        }
        protected void DrawText(XGraphics gfx, Decesion decesion)
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
            XRect innerRect = new XRect(60, 300, 510, 250);
            //tf.DrawString();
            //gfx.DrawString(decesion.Description, font, XBrushes.Black, rect, format,tf);
            tf.DrawString(decesion.Description, font, XBrushes.Black, innerRect, XStringFormats.TopLeft);

            //gfx.DrawString(text, font, XBrushes.Black, 70, 330, format);
            //format.Alignment = XStringAlignment.Far;
            //text = $"{decesion.Name} від {decesion.Date:dd/MM/yyyy}";
            //font = new XFont(fontName, 14, XFontStyle.Regular, options);
            //gfx.DrawString(text, font, XBrushes.Black, rect, format);
            format.Alignment = XStringAlignment.Far;
            format.LineAlignment = XLineAlignment.Far;
            font = new XFont(fontName, 14, XFontStyle.Regular, options);
            //text = $"Поточний статус: {decesion.DecesionStatusType.GetDescription()}";
            gfx.DrawString($"Поточний статус: {decesion.DecesionStatusType.GetDescription()}", font, XBrushes.Black, rect, format);

        }
    }
}