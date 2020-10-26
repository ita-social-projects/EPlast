using System.Reflection.Metadata;

namespace EPlast.BLL
{
    internal interface IPdfDocument
    {
        PdfSharpCore.Pdf.PdfDocument GetDocument();
    }
}