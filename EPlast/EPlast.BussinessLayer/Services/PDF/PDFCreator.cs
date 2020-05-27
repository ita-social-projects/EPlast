using MigraDoc.Rendering;
using System;
using System.IO;
using System.Text;

namespace EPlast.BussinessLayer
{
    internal class PDFCreator : IPDFCreator
    {
        private readonly IPDFDocument document;
        private PdfDocumentRenderer renderer;

        public PDFCreator(IPDFDocument document)
        {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public byte[] GetPDFBytes()
        {
            CreatePDF();
            byte[] fileContents;
            using (var stream = new MemoryStream())
            {
                renderer.PdfDocument.Save(stream, true);
                fileContents = stream.ToArray();
            }

            return fileContents;
        }

        private void CreatePDF()
        {
            renderer = new PdfDocumentRenderer(true)
            {
                Document = document.GetDocument()
            };

            renderer.RenderDocument();
        }
    }
}
