using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.IO;


namespace EPlast.BLL.Services.PDF.Documents
{
    public abstract class PdfDocument : IPdfDocument
    {
        protected PdfSharpCore.Pdf.PdfDocument document;
        private readonly IPdfSettings settings;

        protected PdfDocument() : this(new PdfSettings())
        {
        }

        protected PdfDocument(IPdfSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            document = new PdfSharpCore.Pdf.PdfDocument();
        }
        protected void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            var bytes = Convert.FromBase64String(jpegSamplePath);
            string path = "../decision-tmp.img";
            using (var imageFile = new FileStream(path, FileMode.OpenOrCreate))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
                imageFile.Close();
            }

            XImage image = XImage.FromFile(path);
            gfx.DrawImage(image, x, y, width, height);
            File.Delete(path);
        }

        public virtual PdfSharpCore.Pdf.PdfDocument GetDocument()
        {
            PdfPage page = document.AddPage();
            document.Info.Title = PdfHelper.EncodingHack(settings.Title);
            document.Info.Subject = settings.Subject;
            document.Info.Author = settings.Author;


            XGraphics gfx = XGraphics.FromPdfPage(page);

            if (!settings.ImagePath.Contains("Blank"))
            {
                string base64 = settings.ImagePath.Split(',')[1];
                DrawImage(gfx, base64, 0, 0, 615, 205);
            }
            
            SetDocumentBody(page, gfx);

            return document;
        }

        public abstract void SetDocumentBody(PdfPage page, XGraphics gfx);

    }
}