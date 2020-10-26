using System.Reflection.Metadata;

namespace EPlast.BLL
{
    internal interface IPdfDocument
    {
        PdfSharp.Pdf.PdfDocument GetDocument();
    }
}