using EPlast.BLL.DTO;
using MediatR;

namespace EPlast.BLL.Commands.Decision
{
    public class SaveDecisionAsyncCommand : IRequest<int>
    {      
        public DecisionWrapperDTO Decision { get; set; }
        public SaveDecisionAsyncCommand(DecisionWrapperDTO decisionWrapper)
        {
            Decision = decisionWrapper;
        }
    }
}
