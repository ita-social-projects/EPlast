using EPlast.BLL.DTO;
using MediatR;

namespace EPlast.BLL.Queries.Decision
{
    public class DownloadDecisionFileFromBlobAsyncQuery: IRequest<string>
    {
        public string FileName { get; set; }
        public DownloadDecisionFileFromBlobAsyncQuery(string fileName)
        {
            FileName = fileName;
        }
    }
}
