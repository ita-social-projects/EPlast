using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;

namespace EPlast.BussinessLayer
{
    public abstract class PdfDocument : IPDFDocument
    {
        private readonly IPDFSettings settings;
        protected readonly Document document;

        public PdfDocument() : this(new PDFSettings())
        {
        }

        public PdfDocument(IPDFSettings settings)
        {
            this.settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
            this.document = new Document();
        }

        public abstract void SetDocumentBody(Section section);

        public virtual Document GetDocument()
        {
            Section section;
            document.Info.Title = settings.Title;
            document.Info.Subject = settings.Subject;
            document.Info.Author = settings.Author;

            DefineStyles(document);

            section = document.AddSection();
            Image image = section.AddImage(settings.ImagePath);
            image.Width = 600;
            image.RelativeHorizontal = RelativeHorizontal.Page;
            image.RelativeVertical = RelativeVertical.Page;

            SetDocumentBody(section);

            return document;
        }

        public virtual void DefineStyles(Document document)
        {
            Style style = document.Styles[settings.StyleName];
            style.Font.Name = settings.FontName;
        }
    }
}