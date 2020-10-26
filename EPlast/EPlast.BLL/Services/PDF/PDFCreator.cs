using MigraDoc.Rendering;
using System;
using System.IO;
using System.Text;

namespace EPlast.BLL
{
    internal class PdfCreator : IPdfCreator
    {
        private readonly IPdfDocument document;
        private PdfSharp.Pdf.PdfDocument pdf;

        public PdfCreator(IPdfDocument document)
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
                pdf.Save(stream, true);
                fileContents = stream.ToArray();
            }

            return fileContents;
        }

        private void CreatePDF()
        {
            pdf = document.GetDocument();
        }
    }
}