using EPlast.BLL.ExtensionMethods;
using EPlast.DataAccess.Entities;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

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

            //paragraph.Format = new ParagraphFormat
            //{
            //    Font = new Font
            //    {
            //        Size = 14
            //    },
            //    SpaceAfter = "3cm",
            //    SpaceBefore = "5cm",
            //    Alignment = ParagraphAlignment.Right
            //};

            //paragraph = section.AddParagraph(decesion.Description);
            //paragraph.Format = new ParagraphFormat
            //{
            //    Font = new Font
            //    {
            //        Size = 12
            //    },
            //    SpaceAfter = "1cm",
            //};

            //paragraph = section.AddParagraph($"Поточний статус: {decesion.DecesionStatusType.GetDescription()}");
            //paragraph.Format = new ParagraphFormat
            //{
            //    Font = new Font
            //    {
            //        Size = 14
            //    },
            //    SpaceBefore = "5cm",
            //    Alignment = ParagraphAlignment.Right
            //};
        }
        protected void DrawText(XGraphics gfx, Decesion decesion)
        {
            const string facename = "Times New Roman";

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            XFont font = new XFont(facename, 12, XFontStyle.Regular, options);

            XStringFormat format = new XStringFormat();
            string text = $"{decesion.Description}";

            gfx.DrawString(text, font, XBrushes.Black, 70, 330, format);

            text = $"{decesion.Name} від {decesion.Date:dd/MM/yyyy}";
            font = new XFont(facename, 14, XFontStyle.Regular, options);
            gfx.DrawString(text, font, XBrushes.Black, 375, 220, format);

            text = $"Поточний статус: {decesion.DecesionStatusType.GetDescription()}";
            gfx.DrawString(text, font, XBrushes.Black, 350, 480, format);

        }
    }
}