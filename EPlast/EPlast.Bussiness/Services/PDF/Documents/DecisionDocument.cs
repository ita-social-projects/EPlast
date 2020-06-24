using EPlast.Bussiness.ExtensionMethods;
using EPlast.DataAccess.Entities;
using MigraDoc.DocumentObjectModel;

namespace EPlast.Bussiness
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