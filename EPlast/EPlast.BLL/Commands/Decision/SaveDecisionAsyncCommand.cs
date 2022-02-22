using EPlast.BLL.DTO;
using MediatR;

namespace EPlast.BLL.Commands.Decision
{
    public class SaveDecisionAsyncCommand : IRequest<int>
    {      
        public DecisionWrapperDTO decision { get; set; }
        public SaveDecisionAsyncCommand(DecisionWrapperDTO _decisionWrapper)
        {
            decision = _decisionWrapper;
        }
    }
}
