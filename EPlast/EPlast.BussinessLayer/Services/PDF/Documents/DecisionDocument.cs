using EPlast.BussinessLayer.ExtensionMethods;
using EPlast.DataAccess.Entities;
using MigraDoc.DocumentObjectModel;

namespace EPlast.BussinessLayer
{
    public class DecisionDocument : PdfDocument
    {
        private readonly Decesion decesion;
        private readonly IPDFSettings settings;

        public DecisionDocument(Decesion decesion) : this(decesion, new PDFSettings())
        {
        }

        public DecisionDocument(Decesion decesion, IPDFSettings settings) : base(settings)
        {
            this.decesion = decesion;
            this.settings = settings;
        }

        public override void SetDocumentBody(Section section)
        {
            var paragraph = section.AddParagraph($"{decesion.Name} від {decesion.Date.ToString("dd/MM/yyyy")}");
            paragraph.Format.Font.Size = 14;
            paragraph.Format.SpaceAfter = "3cm";
            paragraph.Format.SpaceBefore = "5cm";
            paragraph.Format.Alignment = ParagraphAlignment.Right;

            paragraph = section.AddParagraph(decesion.Description);
            paragraph.Format.Font.Size = 12;
            paragraph.Format.SpaceAfter = "1cm";

            paragraph = section.AddParagraph($"Поточний статус: {decesion.DecesionStatusType.GetDescription()}");
            paragraph.Format.Font.Size = 14;
            paragraph.Format.SpaceBefore = "5cm";
            paragraph.Format.Alignment = ParagraphAlignment.Right;
        }
    }
}