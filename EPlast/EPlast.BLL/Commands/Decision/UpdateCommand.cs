using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities.Decision;
using MediatR;

namespace EPlast.BLL.Commands.Decision
{
    public class UpdateCommand: IRequest
    {
        public DecisionDto DecisionDto { get; set; }
        public UpdateCommand(DecisionDto decisionDTO)
        {
            DecisionDto = decisionDTO;
        }
    }
}
