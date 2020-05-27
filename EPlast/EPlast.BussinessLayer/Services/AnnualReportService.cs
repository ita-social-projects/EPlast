using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Exceptions;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Services
{
    public class AnnualReportService : IAnnualReportService
    {
        private const string CityAdminTypeName = "Голова Станиці";
        private const string ErrorMessageNoAccess = "Ви не маєте доступу до даного звіту!";
        private const string ErrorMessageHasCreated = "Річний звіт для даної станиці вже створений!";
        private const string ErrorMessageHasUnconfirmed = "Станиця має непідтверджені звіти!";
        private const string ErrorMessageEditFailed = "Не вдалося редагувати річний звіт!";

        private readonly AdminType _cityAdminType;

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        private readonly ICityAccessService _cityAccessService;
        private readonly IMapper _mapper;

        public AnnualReportService(IRepositoryWrapper repositoryWrapper, UserManager<User> userManager, ICityAccessService cityAccessService, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _cityAccessService = cityAccessService;
            _mapper = mapper;
            _cityAdminType = _repositoryWrapper.AdminType
                .FindByCondition(at => at.AdminTypeName == CityAdminTypeName)
                .FirstOrDefault();
        }

        public async Task<AnnualReportDTO> GetByIdAsync(ClaimsPrincipal claimsPrincipal, int id)
        {
            var annualReport = _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.ID == id)
                .Include(ar => ar.MembersStatistic)
                .Include(ar => ar.CityManagement)
                .Include(ar => ar.City)
                .First();
            return await _cityAccessService.HasAccessAsync(claimsPrincipal, annualReport.CityId) ? _mapper.Map<AnnualReport, AnnualReportDTO>(annualReport)
                : throw new AnnualReportException(ErrorMessageNoAccess);
        }

        public async Task<IEnumerable<AnnualReportDTO>> GetAllAsync(ClaimsPrincipal claimsPrincipal)
        {
            var annualReports = await _repositoryWrapper.AnnualReports
                .FindAll()
                .Include(ar => ar.User)
                .Include(ar => ar.City)
                    .ThenInclude(c => c.Region)
                .ToListAsync();
            var citiesDTO = await _cityAccessService.GetCitiesAsync(claimsPrincipal);
            var filteredAnnualReports = annualReports.Where(ar => citiesDTO.Any(c => c.ID == ar.CityId));
            return _mapper.Map<IEnumerable<AnnualReport>, IEnumerable<AnnualReportDTO>>(filteredAnnualReports);
        }

        public async Task CreateAsync(ClaimsPrincipal claimsPrincipal, AnnualReportDTO annualReportDTO)
        {
            if (await _cityAccessService.HasAccessAsync(claimsPrincipal, annualReportDTO.CityId))
            {
                throw new AnnualReportException(ErrorMessageNoAccess);
            }
            if (this.HasCreated(annualReportDTO.CityId))
            {
                throw new AnnualReportException(ErrorMessageHasCreated);
            }
            if (this.HasUnconfirmed(annualReportDTO.CityId))
            {
                throw new AnnualReportException(ErrorMessageHasUnconfirmed);
            }
            var annualReport = _mapper.Map<AnnualReportDTO, AnnualReport>(annualReportDTO);
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            annualReport.UserId = user.Id;
            annualReport.Date = DateTime.Now;
            annualReport.Status = DatabaseEntities.AnnualReportStatus.Unconfirmed;
            _repositoryWrapper.AnnualReports.Create(annualReport);
            _repositoryWrapper.Save();
        }

        public async Task EditAsync(ClaimsPrincipal claimsPrincipal, AnnualReportDTO annualReportDTO)
        {
            var annualReport = _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.ID == annualReportDTO.ID && ar.CityId == annualReportDTO.CityId && ar.UserId == annualReportDTO.UserId
                && ar.Date == annualReportDTO.Date && ar.Status == DatabaseEntities.AnnualReportStatus.Unconfirmed)
                .FirstOrDefault();
            if (annualReport == null || annualReportDTO.Status != DTO.AnnualReportStatus.Unconfirmed)
            {
                throw new AnnualReportException(ErrorMessageEditFailed);
            }
            if (await _cityAccessService.HasAccessAsync(claimsPrincipal, annualReport.CityId))
            {
                throw new AnnualReportException(ErrorMessageNoAccess);
            }
            annualReport = _mapper.Map<AnnualReportDTO, AnnualReport>(annualReportDTO);
            _repositoryWrapper.AnnualReports.Update(annualReport);
            _repositoryWrapper.Save();
        }

        public async Task ConfirmAsync(ClaimsPrincipal claimsPrincipal, int id)
        {
            var annualReport = _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.ID == id && ar.Status == DatabaseEntities.AnnualReportStatus.Unconfirmed)
                .Include(ar => ar.CityManagement)
                    .ThenInclude(cm => cm.CityAdminNew)
                .First();
            if (await _cityAccessService.HasAccessAsync(claimsPrincipal, annualReport.CityId))
            {
                throw new AnnualReportException(ErrorMessageNoAccess);
            }
            annualReport.Status = DatabaseEntities.AnnualReportStatus.Confirmed;
            await this.ChangeCityAdministrationAsync(annualReport.CityId, annualReport.CityManagement.CityAdminNew);
            this.ChangeCityLegalStatus(annualReport.CityId, annualReport.CityManagement.CityLegalStatusNew);
            this.SaveLastConfirmed(annualReport.CityId);
            _repositoryWrapper.Save();
        }

        public async Task CancelAsync(ClaimsPrincipal claimsPrincipal, int id)
        {
            var annualReport = _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.ID == id && ar.Status == DatabaseEntities.AnnualReportStatus.Confirmed)
                .Include(ar => ar.CityManagement)
                .First();
            if (await _cityAccessService.HasAccessAsync(claimsPrincipal, annualReport.CityId))
            {
                throw new AnnualReportException(ErrorMessageNoAccess);
            }
            annualReport.Status = DatabaseEntities.AnnualReportStatus.Unconfirmed;
            var cityAdministrationRevertPoint = annualReport.CityManagement.CityAdminOldId ?? default;
            var cityLegalStatusRevertPoint = annualReport.CityManagement.CityLegalStatusOldId ?? default;
            annualReport.CityManagement.CityAdminOldId = annualReport.CityManagement.CityLegalStatusOldId = null;
            await this.RevertCityAdministrationAsync(cityAdministrationRevertPoint, annualReport.CityId);
            this.RevertCityLegalStatus(cityLegalStatusRevertPoint, annualReport.CityId);
            _repositoryWrapper.Save();
        }

        public async Task DeleteAsync(ClaimsPrincipal claimsPrincipal, int id)
        {
            var annualReport = _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.ID == id && ar.Status == DatabaseEntities.AnnualReportStatus.Unconfirmed)
                .First();
            if (await _cityAccessService.HasAccessAsync(claimsPrincipal, annualReport.CityId))
            {
                throw new AnnualReportException(ErrorMessageNoAccess);
            }
            _repositoryWrapper.AnnualReports.Delete(annualReport);
            _repositoryWrapper.Save();
        }

        public bool HasUnconfirmed(int cityId)
        {
            var annualReport = _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.CityId == cityId && ar.Status == DatabaseEntities.AnnualReportStatus.Unconfirmed)
                .FirstOrDefault();
            return annualReport != null;
        }

        public bool HasCreated(int cityId)
        {
            var annualReport = _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.CityId == cityId && ar.Date.Year == DateTime.Now.Year)
                .FirstOrDefault();
            return annualReport != null;
        }

        private void SaveLastConfirmed(int cityId)
        {
            var annualReport = _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.CityId == cityId && ar.Status == DatabaseEntities.AnnualReportStatus.Confirmed)
                .FirstOrDefault();
            if (annualReport != null)
            {
                annualReport.Status = DatabaseEntities.AnnualReportStatus.Saved;
                _repositoryWrapper.AnnualReports.Update(annualReport);
            }
        }

        private async Task ChangeCityAdministrationAsync(int cityId, User newCityAdministrationUser)
        {
            var oldCityAdministration = _repositoryWrapper.CityAdministration
                .FindByCondition(ca => ca.CityId == cityId && ca.EndDate == null && ca.AdminTypeId == _cityAdminType.ID)
                .Include(ca => ca.User)
                .FirstOrDefault();
            if (oldCityAdministration != null && oldCityAdministration.UserId != newCityAdministrationUser.Id)
            {
                oldCityAdministration.EndDate = DateTime.Now;
                _repositoryWrapper.CityAdministration.Update(oldCityAdministration);
                await _userManager.RemoveFromRoleAsync(oldCityAdministration.User, _cityAdminType.AdminTypeName);
            }
            if (oldCityAdministration == null || oldCityAdministration.UserId != newCityAdministrationUser.Id)
            {
                _repositoryWrapper.CityAdministration.Create(new CityAdministration
                {
                    CityId = cityId,
                    UserId = newCityAdministrationUser.Id,
                    AdminTypeId = _cityAdminType.ID,
                    StartDate = DateTime.Now
                });
                await _userManager.AddToRoleAsync(newCityAdministrationUser, _cityAdminType.AdminTypeName);
            }
        }

        private void ChangeCityLegalStatus(int cityId, DatabaseEntities.CityLegalStatusType newCityLegalStatusType)
        {
            var oldCityLegalStatus = _repositoryWrapper.CityLegalStatuses
                .FindByCondition(cls => cls.CityId == cityId && cls.DateStart == null)
                .FirstOrDefault();
            if (oldCityLegalStatus != null && oldCityLegalStatus.LegalStatusType != newCityLegalStatusType)
            {
                oldCityLegalStatus.DateFinish = DateTime.Now;
                _repositoryWrapper.CityLegalStatuses.Update(oldCityLegalStatus);
            }
            if (oldCityLegalStatus == null || oldCityLegalStatus.LegalStatusType != newCityLegalStatusType)
            {
                _repositoryWrapper.CityLegalStatuses.Create(new CityLegalStatus
                {
                    CityId = cityId,
                    LegalStatusType = newCityLegalStatusType,
                    DateStart = DateTime.Now
                });
            }
        }

        private async Task RevertCityAdministrationAsync(int revertPointId, int cityId)
        {
            var cityAdministraions = _repositoryWrapper.CityAdministration
                .FindByCondition(ca => ca.ID > revertPointId && ca.CityId == cityId && ca.AdminTypeId == _cityAdminType.ID)
                .Include(ca => ca.User);
            foreach (var cityAdministration in cityAdministraions)
            {
                _repositoryWrapper.CityAdministration.Delete(cityAdministration);
                await _userManager.RemoveFromRoleAsync(cityAdministration.User, _cityAdminType.AdminTypeName);
            }
            var cityAdministrationRevertPoint = _repositoryWrapper.CityAdministration
                .FindByCondition(ca => ca.ID == revertPointId)
                .Include(ca => ca.User)
                .FirstOrDefault();
            if (cityAdministrationRevertPoint != null)
            {
                cityAdministrationRevertPoint.EndDate = null;
                _repositoryWrapper.CityAdministration.Update(cityAdministrationRevertPoint);
                await _userManager.AddToRoleAsync(cityAdministrationRevertPoint.User, _cityAdminType.AdminTypeName);
            }
        }

        private void RevertCityLegalStatus(int revertPointId, int cityId)
        {
            var cityLegalStatuses = _repositoryWrapper.CityLegalStatuses
                .FindByCondition(cls => cls.Id > revertPointId && cls.CityId == cityId);
            foreach (var cityLegalStatus in cityLegalStatuses)
            {
                _repositoryWrapper.CityLegalStatuses.Delete(cityLegalStatus);
            }
            var cityLegalStatusRevertPoint = _repositoryWrapper.CityLegalStatuses
                .FindByCondition(cls => cls.Id == revertPointId)
                .FirstOrDefault();
            if (cityLegalStatusRevertPoint != null)
            {
                cityLegalStatusRevertPoint.DateFinish = null;
                _repositoryWrapper.CityLegalStatuses.Update(cityLegalStatusRevertPoint);
            }
        }
    }
}