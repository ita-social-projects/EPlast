using System.Threading.Tasks;

namespace EPlast.BLL

{
    public interface IPdfService
    {
        Task<byte[]> DecisionCreatePDFAsync(int DecisionId);

        Task<byte[]> BlankCreatePDFAsync(string userId);
    }
}