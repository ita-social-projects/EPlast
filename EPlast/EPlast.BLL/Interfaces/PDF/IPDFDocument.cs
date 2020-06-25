using MigraDoc.DocumentObjectModel;
using System.Reflection.Metadata;
using Document = MigraDoc.DocumentObjectModel.Document;

namespace EPlast.BLL
{
    internal interface IPdfDocument
    {
        Document GetDocument();
    }
}