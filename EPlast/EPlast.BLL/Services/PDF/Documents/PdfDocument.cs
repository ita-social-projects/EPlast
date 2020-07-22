using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using System;

namespace EPlast.BLL
{
    public abstract class PdfDocument : IPdfDocument
    {
        protected readonly Document Document;
        private readonly IPdfSettings settings;

        protected PdfDocument() : this(new PdfSettings())
        {
        }

        protected PdfDocument(IPdfSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Document = new Document();
        }

        public virtual Document GetDocument()
        {
            Section section;
            Document.Info.Title = settings.Title;
            Document.Info.Subject = settings.Subject;
            Document.Info.Author = settings.Author;

            DefineStyles(Document);

            section = Document.AddSection();

            if (!settings.ImagePath.Contains("Blank"))
            {
                string base64 = "base64:" + settings.ImagePath.Split(',')[1];
                Image image = section.AddImage(base64);
                image.Width = 600;
                image.RelativeHorizontal = RelativeHorizontal.Page;
                image.RelativeVertical = RelativeVertical.Page;
            }
            else
            {
                Image image = section.AddImage(settings.ImagePath);
                image.Width = 84;
                image.Left = 40;
                image.Top = 20;
                image.RelativeHorizontal = RelativeHorizontal.Page;
                image.RelativeVertical = RelativeVertical.Page;
            }
            SetDocumentBody(section);

            return Document;
        }

        public abstract void SetDocumentBody(Section section);

        public virtual void DefineStyles(Document document)
        {
            var style = document.Styles[settings.StyleName];
            style.Font.Name = settings.FontName;
        }
    }
}