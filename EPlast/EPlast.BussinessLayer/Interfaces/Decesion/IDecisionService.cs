using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public interface IDecisionService
    {
        Task<DecisionDto> GetDecision(int id);

        List<DecisionDto> GetDecisionList();

        Task<bool> CreateDecision(DecisionDto decision);

        Task<bool> ChangeDecision(DecisionDto decision);

        Task<bool> SaveDecisionAsync();

        Task<byte[]> DownloadDecision();
    }
}