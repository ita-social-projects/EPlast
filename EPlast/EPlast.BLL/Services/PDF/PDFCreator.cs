using System;
using System.IO;
using System.Text;

namespace EPlast.BLL.Services.PDF
{
    internal class PdfCreator : IPdfCreator
    {
        private readonly IPdfDocument document;
        private PdfSharpCore.Pdf.PdfDocument pdf;

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