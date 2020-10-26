using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;


namespace EPlast.BLL
{
    public abstract class PdfDocument : IPdfDocument
    {
        protected PdfSharp.Pdf.PdfDocument document;
        private XGraphicsState state;
        private readonly IPdfSettings settings;

        protected PdfDocument() : this(new PdfSettings())
        {
        }

        protected PdfDocument(IPdfSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            document = new PdfSharp.Pdf.PdfDocument();
        }
        protected void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            var bytes = Convert.FromBase64String(jpegSamplePath);
            var ph = Convert.FromBase64String(jpegSamplePath);
            string path = "../decisiontmp.img";
            using (var imageFile = new FileStream(path, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }

            XImage image = XImage.FromFile(path);
            gfx.DrawImage(image, x, y, width, height);
        }

        public virtual PdfSharp.Pdf.PdfDocument GetDocument()
        {
            PdfPage page = document.AddPage();
            document.Info.Title = settings.Title;
            document.Info.Subject = settings.Subject;
            document.Info.Author = settings.Author;

            DefineStyles(document);

            XGraphics gfx = XGraphics.FromPdfPage(page);

            if (!settings.ImagePath.Contains("Blank"))
            {
                string base64 = settings.ImagePath.Split(',')[1];
                DrawImage(gfx, base64, 0, 0, 615, 205);
            }
            else
            {
                DrawImage(gfx, settings.ImagePath, 40, 20, 84, 250);
            }
            SetDocumentBody(page, gfx);

            return document;
        }

        public abstract void SetDocumentBody(PdfPage page, XGraphics gfx);

        public virtual void DefineStyles(PdfSharp.Pdf.PdfDocument document)
        {
            //var style = document.Styles[settings.StyleName];
            //style.Font.Name = settings.FontName;
        }
    }
}