using EPlast.DataAccess.Entities;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public interface IPDFService
    {
        Task<byte[]> DecesionCreatePDFAsync(Decesion pdfData);
    }
}