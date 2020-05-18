using EPlast.BussinessLayer.ExtensionMethods;
using EPlast.DataAccess.Entities;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;

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
            var paragraph = section.AddParagraph($"{decesion.Name} від {decesion.Date:dd/MM/yyyy}");

            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Size = 14
                },
                SpaceAfter = "3cm",
                SpaceBefore = "5cm",
                Alignment = ParagraphAlignment.Right
            };

            paragraph = section.AddParagraph(decesion.Description);
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Size = 12
                },
                SpaceAfter = "1cm",
            };

            paragraph = section.AddParagraph($"Поточний статус: {decesion.DecesionStatusType.GetDescription()}");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Size = 14
                },
                SpaceBefore = "5cm",
                Alignment = ParagraphAlignment.Right
            };
        }
    }
}