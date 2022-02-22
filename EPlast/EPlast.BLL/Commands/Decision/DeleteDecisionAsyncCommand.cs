using EPlast.BLL.DTO;
using MediatR;

namespace EPlast.BLL.Commands.Decision
{
    public class DeleteDecisionAsyncCommand: IRequest
    {
        public int id { get; set; }
        public DeleteDecisionAsyncCommand(int _id)
        {
            id = _id;
        }
    }
}
