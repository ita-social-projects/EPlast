using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using System;

namespace EPlast.BussinessLayer
{
    public abstract class PdfDocument : IPDFDocument
    {
        protected readonly Document document;
        private readonly IPDFSettings settings;

        protected PdfDocument() : this(new PDFSettings())
        {
        }

        protected PdfDocument(IPDFSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            document = new Document();
        }

        public virtual Document GetDocument()
        {
            Section section;
            document.Info.Title = settings.Title;
            document.Info.Subject = settings.Subject;
            document.Info.Author = settings.Author;

            DefineStyles(document);

            section = document.AddSection();
            var image = section.AddImage(settings.ImagePath);
            image.Width = 600;
            image.RelativeHorizontal = RelativeHorizontal.Page;
            image.RelativeVertical = RelativeVertical.Page;

            SetDocumentBody(section);

            return document;
        }

        public abstract void SetDocumentBody(Section section);

        public virtual void DefineStyles(Document document)
        {
            var style = document.Styles[settings.StyleName];
            style.Font.Name = settings.FontName;
        }
    }
}