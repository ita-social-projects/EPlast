using System.Threading.Tasks;

namespace EPlast.BussinessLayer

{
    public interface IPdfService
    {
        Task<byte[]> DecisionCreatePDFAsync(int DecisionId);

        Task<byte[]> BlankCreatePDFAsync(string userId);
    }
}