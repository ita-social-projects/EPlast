using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities.Decision;
using MediatR;

namespace EPlast.BLL.Commands.Decision
{
    public class UpdateCommand: IRequest
    {
        public DecisionDTO DecisionDto { get; set; }
        public UpdateCommand(DecisionDTO decisionDTO)
        {
            DecisionDto = decisionDTO;
        }
    }
}
