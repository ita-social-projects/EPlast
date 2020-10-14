using System.Threading.Tasks;

namespace EPlast.BLL

{
    public interface IPdfService
    {
        Task<byte[]> DecisionCreatePDFAsync(int decisionId);

        Task<byte[]> BlankCreatePDFAsync(string userId);
    }
}