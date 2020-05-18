using EPlast.DataAccess.Entities;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public class PDFService : IPDFService
    {
        public async Task<byte[]> DecesionCreatePDFAsync(Decesion pdfData)
        {
            IPDFSettings pdfSettings = new PDFSettings()
            {
                Title = $"Рішення {pdfData.Organization.OrganizationName}"
            };
            IPDFCreator creator = new PDFCreator(new DecisionDocument(pdfData, pdfSettings));
            return await Task.Run(() => creator.GetPDFBytes());
        }
    }
}