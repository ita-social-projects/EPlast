using System.Threading.Tasks;

namespace EPlast.BussinessLayer

{
    public interface IPDFService
    {
        Task<byte[]> DecesionCreatePDFAsync(Decesion pdfData);

        Task<byte[]> BlankCreatePDFAsync(BlankModel pdfData);
    }
}