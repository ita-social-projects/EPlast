using EPlast.BussinessLayer.ExtensionMethods;
using EPlast.DataAccess.Entities;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;

namespace EPlast.BussinessLayer
{
    public class BlankDocument : PdfDocument
    {
        private readonly BlankModel blank;
        private readonly IPDFSettings settings;

        public BlankDocument(BlankModel blank) : this(blank, new PDFSettings())
        {
        }

        public BlankDocument(BlankModel blank, IPDFSettings settings) : base(settings)
        {
            this.blank = blank;
            this.settings = settings;
        }

        public override void SetDocumentBody(Section section)
        {
            //var paragraph = section.AddParagraph($"{blank.Name} від {blank.Date.ToString("dd/MM/yyyy")}");
            //paragraph.Format.Font.Size = 14;
            //paragraph.Format.SpaceAfter = "3cm";
            //paragraph.Format.SpaceBefore = "5cm";
            //paragraph.Format.Alignment = ParagraphAlignment.Right;

            //paragraph = section.AddParagraph(blank.Description);
            //paragraph.Format.Font.Size = 12;
            //paragraph.Format.SpaceAfter = "1cm";

            //paragraph = section.AddParagraph($"Поточний статус: {blank.DecesionStatusType.GetDescription()}");
            //paragraph.Format.Font.Size = 14;
            //paragraph.Format.SpaceBefore = "5cm";
            //paragraph.Format.Alignment = ParagraphAlignment.Right;
        }
    }
}