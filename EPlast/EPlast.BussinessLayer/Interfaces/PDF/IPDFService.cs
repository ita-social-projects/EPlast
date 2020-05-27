using System.Threading.Tasks;

namespace EPlast.BussinessLayer

{
    public interface IPDFService
    {
        Task<byte[]> DecisionCreatePDFAsync(int DecisionId);

        Task<byte[]> BlankCreatePDFAsync(string userId);
    }
}