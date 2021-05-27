using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.PDF.Documents;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

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
                var blank = await GetBlankDataAsync(userId);
                IPdfSettings pdfSettings = new PdfSettings
                {
                    Title = $"{blank.User.FirstName} {blank.User.LastName}",
                    ImagePath = "Blank"
                };
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
                    var base64 = await _decisionBlobStorage.GetBlobBase64Async("dafaultPhotoForPdf.jpg");
                    IPdfSettings pdfSettings = new PdfSettings
                    {
                        Title = $"Decision of {decision.Organization.OrganizationName}",
                        ImagePath = base64,
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

        public async Task<byte[]> MethodicDocumentCreatePdfAsync(int methodicDocumentId)
        {
            try
            {
                var methodicDocument = await _repoWrapper.MethodicDocument.GetFirstAsync(x => x.ID == methodicDocumentId, include: doc =>
                    doc.Include(d => d.Organization));
                if (methodicDocument != null)
                {
                    var base64 = await _decisionBlobStorage.GetBlobBase64Async("dafaultPhotoForPdf.jpg");
                    IPdfSettings pdfSettings = new PdfSettings
                    {
                        Title = $"{methodicDocument.Type} від {methodicDocument.Organization.OrganizationName}",
                        ImagePath = base64,
                    };
                    IPdfCreator creator = new PdfCreator(new MethodicDocumentPdf(methodicDocument, pdfSettings));
                    return await Task.Run(() => creator.GetPDFBytes());
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return null;
        }

        private async Task<BlankModel> GetBlankDataAsync(string userId)
        {
            var user = await _repoWrapper.User.GetFirstOrDefaultAsync(predicate: c => c.Id == userId,
                include: source => source
                    .Include(c => c.ConfirmedUsers)
                        .ThenInclude(c => c.Approver)
                            .ThenInclude(c => c.User)
                                .ThenInclude(c => c.ClubMembers)
                                    .ThenInclude(c => c.Club)
                    .Include(c => c.UserMembershipDates)
                    .Include(c => c.UserPlastDegrees)
                    .Include(c => c.Participants)
                    .ThenInclude(c => c.Event)
                    .ThenInclude(c => c.EventCategory)
                    .ThenInclude(c => c.EventSection));
            var userProfile = await _repoWrapper.UserProfile
                .GetFirstOrDefaultAsync(predicate: c => c.UserID == userId,
                    include: source => source
                       .Include(c => c.Education)
                       .Include(c => c.Work));
            var cityMembers = await _repoWrapper.CityMembers
                .GetFirstOrDefaultAsync(predicate: c => c.UserId == userId,
                    include: source => source
                       .Include(c => c.City));
            var clubMembers = await _repoWrapper.ClubMembers
                .GetFirstOrDefaultAsync(predicate: c => c.UserId == userId,
                    include: source => source
                       .Include(c => c.Club));

            return new BlankModel
            {
                User = user,
                UserProfile = userProfile,
                CityMembers = cityMembers,
                ClubMembers = clubMembers,
            };
        }
    }
}
