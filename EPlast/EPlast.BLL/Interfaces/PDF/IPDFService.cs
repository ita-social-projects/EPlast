using System.Threading.Tasks;

namespace EPlast.BLL

{
    public interface IPdfService
    {
        Task<byte[]> DecisionCreatePDFAsync(int decisionId);

        Task<byte[]> MethodicDocumentCreatePdfAsync(int methodicDocumentId);

        Task<byte[]> BlankCreatePDFAsync(string userId);

        Task<byte[]> AnnualReportCreatePDFAsync(int annualReportId);
    }
}
