using MigraDoc.Rendering;
using System.IO;

namespace EPlast.BussinessLayer
{
    internal class PDFCreator : IPDFCreator
    {
        private PdfDocumentRenderer renderer;
        private readonly IPDFDocument document;

        public PDFCreator(IPDFDocument document)
        {
            this.document = document ?? throw new System.ArgumentNullException(nameof(document));
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public byte[] GetPDFBytes()
        {
            CreatPDF();
            byte[] fileContents = null;
            using (MemoryStream stream = new MemoryStream())
            {
                renderer.PdfDocument.Save(stream, true);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }

        private void CreatPDF()
        {
            renderer = new PdfDocumentRenderer(true)
            {
                Document = document.GetDocument()
            };

            renderer.RenderDocument();
        }
    }
}