using System;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Repositories;

namespace EPlast.BussinessLayer
{
    public class PDFService : IPDFService
    {
        private readonly IRepositoryWrapper _repoWrapper;

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
                Title = $"Рішення {decision.Organization.OrganizationName}"
            };
            IPDFCreator creator = new PDFCreator(new DecisionDocument(decision, pdfSettings));
            return await Task.Run(() => creator.GetPDFBytes());
        }
    }
}