using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public interface IDecisionService
    {
        Task<DecisionDto> GetDecision(int decisionId);

        Task<List<DecisionDto>> GetDecisionList();

        Task<bool> ChangeDecision(DecisionDto decision);

        Task<bool> SaveDecision(DecisionDto decision);

        Task<byte[]> DownloadDecisionFile(int decisionId);
    }
}