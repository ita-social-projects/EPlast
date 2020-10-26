using System;
using EPlast.DataAccess.Repositories;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL
{
    public class PdfService : IPdfService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ILoggerService<PdfService> _logger;
        private readonly IDecisionBlobStorageRepository _decisionBlobStorage;
        public PdfService(IRepositoryWrapper repoWrapper, ILoggerService<PdfService> logger, IDecisionBlobStorageRepository decisionBlobStorage)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _decisionBlobStorage = decisionBlobStorage;
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

        public async Task<byte[]> DecisionCreatePDFAsync(int decisionId)
        {
            try
            {
                var decision = await _repoWrapper.Decesion.GetFirstAsync(x => x.ID == decisionId, include: dec =>
                    dec.Include(d => d.DecesionTarget).Include(d => d.Organization));
                if (decision != null)
                {
                    //var base64 = await _decisionBlobStorage.GetBlobBase64Async("dafaultPhotoForPdf.jpg");
                    IPdfSettings pdfSettings = new PdfSettings
                    {
                        Title = $"Рішення {decision.Organization.OrganizationName}",
                        //ImagePath = base64
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