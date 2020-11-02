using System.Threading.Tasks;

namespace EPlast.BLL

{
    public interface IPdfService
    {
        Task<byte[]> DecisionCreatePDFAsync(int decisionId);

        Task<string> BlankCreatePDFAsync(string userId);
    }
}