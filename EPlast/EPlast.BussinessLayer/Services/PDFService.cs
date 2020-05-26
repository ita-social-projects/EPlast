using System;
using EPlast.DataAccess.Repositories;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BussinessLayer.Services.Interfaces;

namespace EPlast.BussinessLayer
{
    public class PDFService : IPDFService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ILoggerService<PDFService> _logger;

        public PDFService(IRepositoryWrapper repoWrapper, ILoggerService<PDFService> logger)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
        }

        public async Task<byte[]> BlankCreatePDFAsync(int userId)
        {
            IPDFSettings pdfSettings = new PDFSettings()
            {
                Title = "Бланк",
                ImagePath = "wwwroot/images/pdf/Header-Eplast-Blank.png"
            };
            var blank = GetBlankData(userId);
            IPDFCreator creator = new PDFCreator(new BlankDocument(blank, pdfSettings));
            return await Task.Run(() => creator.GetPDFBytes());
        }

        public async Task<byte[]> DecisionCreatePDFAsync(int DecisionId)
        {
            try
            {
                var decision = _repoWrapper.Decesion.Include(x => x.DecesionTarget, x => x.Organization)
                    .FirstOrDefault(x => x.ID == DecisionId);
                if (decision != null)
                {
                    IPDFSettings pdfSettings = new PDFSettings
                    {
                        Title = $"Рішення {decision.Organization.OrganizationName}",
                        ImagePath = "wwwroot/images/pdf/Header-Eplast.png"
                    };
                    IPDFCreator creator = new PDFCreator(new DecisionDocument(decision, pdfSettings));
                    return await Task.Run(() => creator.GetPDFBytes());
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return null;
        }

        private static BlankModel GetBlankData(int userId)
        {
            return new BlankModel
            {
            };
        }
    }
}