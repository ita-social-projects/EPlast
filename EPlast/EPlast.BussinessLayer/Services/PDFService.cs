using EPlast.DataAccess.Entities;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public class PDFService : IPDFService
    {
        public async Task<byte[]> BlankCreatePDFAsync(BlankModel pdfData)
        {
            IPDFSettings pdfSettings = new PDFSettings()
            {
                Title = string.Format("Бланк"),
                ImagePath = "wwwroot/images/pdf/Header-Eplast-Blank.png"
            };
            IPDFCreator creator = new PDFCreator(new BlankDocument(pdfData, pdfSettings));
            return await Task.Run(() => creator.GetPDFBytes());
        }

        public async Task<byte[]> DecesionCreatePDFAsync(Decesion pdfData)
        {
            IPDFSettings pdfSettings = new PDFSettings()
            {
                Title = string.Format("Рішення {0}", pdfData.Organization.OrganizationName),
                ImagePath = "wwwroot/images/pdf/Header-Eplast.png"
            };
            IPDFCreator creator = new PDFCreator(new DecisionDocument(pdfData, pdfSettings));
            return await Task.Run(() => creator.GetPDFBytes());
        }
    }
}