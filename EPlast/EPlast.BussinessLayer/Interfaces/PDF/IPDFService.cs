using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer
{
    public interface IPDFService
    {
        Task<byte[]> DecisionCreatePDFAsync(int DecisionId);
    }
}