using EPlast.BLL.DTO;
using MediatR;

namespace EPlast.BLL.Commands.Decision
{
    public class CreateDecisionTargetAsyncCommand: IRequest<DecisionTargetDTO>
    {
      public string DecisionTargetName { get; set; }
        public CreateDecisionTargetAsyncCommand(string decisionTargetName)
        {
            DecisionTargetName = decisionTargetName;
        }
    }
}
