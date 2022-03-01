using EPlast.BLL.DTO;
using MediatR;

namespace EPlast.BLL.Commands.Decision
{
    public class UploadFileToBlobAsyncCommand : IRequest
    {
        public string Base64 { get; set; }
        public string FileName { get; set; }
        public UploadFileToBlobAsyncCommand(string base64, string fileName)
        {
            Base64 = base64;
            FileName = fileName;
        }
    }
}
