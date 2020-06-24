using MigraDoc.DocumentObjectModel;
using System.Reflection.Metadata;
using Document = MigraDoc.DocumentObjectModel.Document;

namespace EPlast.BusinessLogicLayer
{
    internal interface IPdfDocument
    {
        Document GetDocument();
    }
}