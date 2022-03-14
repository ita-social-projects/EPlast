using EPlast.BLL.DTO;
using MediatR;

namespace EPlast.BLL.Commands.Decision
{
    public class DeleteDecisionAsyncCommand: IRequest
    {
        public int Id { get; set; }
        public DeleteDecisionAsyncCommand(int id)
        {
            Id = id;
        }
    }
}
