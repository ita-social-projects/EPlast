using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities.Decision;
using MediatR;

namespace EPlast.BLL.Commands.Decision
{
    public class UpdateCommand: IRequest
    {
        public DecisionDTO decisionDTO { get; set; }
        public UpdateCommand(DecisionDTO _decisionDTO)
        {
            decisionDTO = _decisionDTO;
        }
    }
}
