using EPlast.BLL.ExtensionMethods;
using EPlast.DataAccess.Entities;
using PdfSharpCore;
using PdfSharpCore.Drawing;
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
            XFont font = new XFont(fontName, 12, XFontStyle.Regular, options);

            XStringFormat format = new XStringFormat();
            string text = $"{decesion.Description}";

            gfx.DrawString(text, font, XBrushes.Black, 70, 330, format);

            text = $"{decesion.Name} від {decesion.Date:dd/MM/yyyy}";
            font = new XFont(fontName, 14, XFontStyle.Regular, options);
            gfx.DrawString(text, font, XBrushes.Black, 375, 220, format);

            text = $"Поточний статус: {decesion.DecesionStatusType.GetDescription()}";
            gfx.DrawString(text, font, XBrushes.Black, 350, 480, format);

        }
    }
}