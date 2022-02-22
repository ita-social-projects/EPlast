using EPlast.BLL.DTO;
using MediatR;

namespace EPlast.BLL.Commands.Decision
{
    public class UploadFileToBlobAsyncCommand : IRequest
    {
        public string base64 { get; set; }
        public string fileName { get; set; }
        public UploadFileToBlobAsyncCommand(string _base64, string _fileName)
        {
            base64 = _base64;
            fileName = _fileName;
        }
    }
}
