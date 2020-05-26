using EPlast.DataAccess.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public class PDFService : IPDFService
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public async Task<byte[]> BlankCreatePDFAsync(BlankModel pdfData)
        {
            IPDFSettings pdfSettings = new PDFSettings()
            {
                Title = "Бланк",
                ImagePath = "wwwroot/images/pdf/Header-Eplast-Blank.png"
            };
            IPDFCreator creator = new PDFCreator(new BlankDocument(pdfData, pdfSettings));
            return await Task.Run(() => creator.GetPDFBytes());
        }

        public PDFService(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<byte[]> DecisionCreatePDFAsync(int DecisionId)
        {
            var decision = _repoWrapper.Decesion.Include(x => x.DecesionTarget, x => x.Organization)
                .FirstOrDefault(x => x.ID == DecisionId);
            if (decision == null)
                return null;//Fix here
            IPDFSettings pdfSettings = new PDFSettings
            {
                Title = $"Рішення {decision.Organization.OrganizationName}",
                ImagePath = "wwwroot/images/pdf/Header-Eplast.png"
            };
            IPDFCreator creator = new PDFCreator(new DecisionDocument(decision, pdfSettings));
            return await Task.Run(() => creator.GetPDFBytes());
        }
    }
}