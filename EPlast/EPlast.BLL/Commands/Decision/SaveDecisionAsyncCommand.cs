using EPlast.BLL.DTO;
using MediatR;

namespace EPlast.BLL.Commands.Decision
{
    public class SaveDecisionAsyncCommand : IRequest<int>
    {
        public DecisionWrapperDto Decision { get; set; }
        public SaveDecisionAsyncCommand(DecisionWrapperDto decisionWrapper)
        {
            Decision = decisionWrapper;
        }
    }
}
