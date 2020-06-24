using System;
using EPlast.DataAccess.Repositories;
using System.Linq;
using System.Threading.Tasks;
using EPlast.Bussiness.Interfaces.Logging;
using EPlast.Bussiness.Services.Interfaces;

namespace EPlast.Bussiness
{
    public class PdfService : IPdfService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ILoggerService<PdfService> _logger;

        public PdfService(IRepositoryWrapper repoWrapper, ILoggerService<PdfService> logger)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
        }

        public async Task<byte[]> BlankCreatePDFAsync(string userId)
        {
            try
            {
                IPdfSettings pdfSettings = new PdfSettings
                {
                    Title = "Бланк",
                    ImagePath = "wwwroot/images/pdf/Header-Eplast-Blank.png"
                };
                var blank = GetBlankData(userId);
                IPdfCreator creator = new PdfCreator(new BlankDocument(blank, pdfSettings));
                return await Task.Run(() => creator.GetPDFBytes());
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return null;
        }

        public async Task<byte[]> DecisionCreatePDFAsync(int DecisionId)
        {
            try
            {
                var decision = _repoWrapper.Decesion.Include(x => x.DecesionTarget, x => x.Organization)
                    .FirstOrDefault(x => x.ID == DecisionId);
                if (decision != null)
                {
                    IPdfSettings pdfSettings = new PdfSettings
                    {
                        Title = $"Рішення {decision.Organization.OrganizationName}",
                        ImagePath = "wwwroot/images/pdf/Header-Eplast.png"
                    };
                    IPdfCreator creator = new PdfCreator(new DecisionDocument(decision, pdfSettings));
                    return await Task.Run(() => creator.GetPDFBytes());
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return null;
        }

        private BlankModel GetBlankData(string userId)
        {
            var user = _repoWrapper.User.FindByCondition(x => x.Id.Equals(userId)).First();
            var userProfile = _repoWrapper.UserProfile.FindByCondition(x => x.UserID.Equals(userId)).First();
            var cityMembers = _repoWrapper.CityMembers
                .FindByCondition(x => x.UserId.Equals(userId)).First();
            var clubMembers = _repoWrapper.ClubMembers
                .FindByCondition(x => x.UserId.Equals(userId)).First();
            var cityAdmin = _repoWrapper.User.FindByCondition(x =>
                x.Id.Equals(_repoWrapper.CityAdministration.FindByCondition(y => y.CityId == cityMembers.CityId)
                    .Select(y => y.UserId)
                    .First()))
                .First();
            return new BlankModel
            {
                User = user,
                UserProfile = userProfile,
                CityMembers = cityMembers,
                ClubMembers = clubMembers,
                CityAdmin = cityAdmin
            };
        }
    }
}