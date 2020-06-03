﻿using AutoMapper;
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
            var annualReport = await _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.ID == id)
                .Include(ar => ar.MembersStatistic)
                .Include(ar => ar.CityManagement)
                    .ThenInclude(cm => cm.CityAdminNew)
                .Include(ar => ar.City)
                .FirstAsync();
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
            if (!await _cityAccessService.HasAccessAsync(claimsPrincipal, annualReportDTO.CityId))
            {
                throw new AnnualReportException(ErrorMessageNoAccess);
            }
            await this.CheckCreatedAndUnconfirmed(annualReportDTO.CityId);
            var annualReport = _mapper.Map<AnnualReportDTO, AnnualReport>(annualReportDTO);
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            annualReport.UserId = user.Id;
            annualReport.Date = DateTime.Now;
            annualReport.Status = DatabaseEntities.AnnualReportStatus.Unconfirmed;
            await _repositoryWrapper.AnnualReports.CreateAsync(annualReport);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task EditAsync(ClaimsPrincipal claimsPrincipal, AnnualReportDTO annualReportDTO)
        {
            var annualReport = await _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.ID == annualReportDTO.ID && ar.CityId == annualReportDTO.CityId && ar.UserId == annualReportDTO.UserId
                && ar.Date.Date == annualReportDTO.Date.Date && ar.Status == DatabaseEntities.AnnualReportStatus.Unconfirmed)
                .FirstOrDefaultAsync();
            if (annualReport == null || annualReportDTO.Status != DTO.AnnualReportStatus.Unconfirmed)
            {
                throw new AnnualReportException(ErrorMessageEditFailed);
            }
            if (!await _cityAccessService.HasAccessAsync(claimsPrincipal, annualReport.CityId))
            {
                throw new AnnualReportException(ErrorMessageNoAccess);
            }
            annualReport = _mapper.Map<AnnualReportDTO, AnnualReport>(annualReportDTO);
            _repositoryWrapper.AnnualReports.Update(annualReport);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task ConfirmAsync(ClaimsPrincipal claimsPrincipal, int id)
        {
            var annualReport = await _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.ID == id && ar.Status == DatabaseEntities.AnnualReportStatus.Unconfirmed)
                .Include(ar => ar.CityManagement)
                .FirstAsync();
            if (!await _cityAccessService.HasAccessAsync(claimsPrincipal, annualReport.CityId))
            {
                throw new AnnualReportException(ErrorMessageNoAccess);
            }
            annualReport.Status = DatabaseEntities.AnnualReportStatus.Confirmed;
            await this.ChangeCityAdministrationAsync(annualReport);
            await this.ChangeCityLegalStatusAsync(annualReport);
            _repositoryWrapper.AnnualReports.Update(annualReport);
            await this.SaveLastConfirmedAsync(annualReport.CityId);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task CancelAsync(ClaimsPrincipal claimsPrincipal, int id)
        {
            var annualReport = await _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.ID == id && ar.Status == DatabaseEntities.AnnualReportStatus.Confirmed)
                .Include(ar => ar.CityManagement)
                .FirstAsync();
            if (!await _cityAccessService.HasAccessAsync(claimsPrincipal, annualReport.CityId))
            {
                throw new AnnualReportException(ErrorMessageNoAccess);
            }
            annualReport.Status = DatabaseEntities.AnnualReportStatus.Unconfirmed;
            var cityAdministrationRevertPoint = annualReport.CityManagement.CityAdminOldId ?? default;
            var cityLegalStatusRevertPoint = annualReport.CityManagement.CityLegalStatusOldId ?? default;
            annualReport.CityManagement.CityAdminOldId = annualReport.CityManagement.CityLegalStatusOldId = null;
            _repositoryWrapper.AnnualReports.Update(annualReport);
            await this.RevertCityAdministrationAsync(cityAdministrationRevertPoint, annualReport.CityId);
            await this.RevertCityLegalStatusAsync(cityLegalStatusRevertPoint, annualReport.CityId);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task DeleteAsync(ClaimsPrincipal claimsPrincipal, int id)
        {
            var annualReport = await _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.ID == id && ar.Status == DatabaseEntities.AnnualReportStatus.Unconfirmed)
                .FirstAsync();
            if (!await _cityAccessService.HasAccessAsync(claimsPrincipal, annualReport.CityId))
            {
                throw new AnnualReportException(ErrorMessageNoAccess);
            }
            _repositoryWrapper.AnnualReports.Delete(annualReport);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task<bool> HasUnconfirmedAsync(int cityId)
        {
            var annualReport = await _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.CityId == cityId && ar.Status == DatabaseEntities.AnnualReportStatus.Unconfirmed)
                .FirstOrDefaultAsync();
            return annualReport != null;
        }

        public async Task<bool> HasCreatedAsync(int cityId)
        {
            var annualReport = await _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.CityId == cityId && ar.Date.Year == DateTime.Now.Year)
                .FirstOrDefaultAsync();
            return annualReport != null;
        }

        public async Task CheckCreatedAndUnconfirmed(int cityId)
        {
            if (await this.HasCreatedAsync(cityId))
            {
                throw new AnnualReportException(ErrorMessageHasCreated);
            }
            if (await this.HasUnconfirmedAsync(cityId))
            {
                throw new AnnualReportException(ErrorMessageHasUnconfirmed);
            }
        }

        private async Task SaveLastConfirmedAsync(int cityId)
        {
            var annualReport = await _repositoryWrapper.AnnualReports
                .FindByCondition(ar => ar.CityId == cityId && ar.Status == DatabaseEntities.AnnualReportStatus.Confirmed)
                .FirstOrDefaultAsync();
            if (annualReport != null)
            {
                annualReport.Status = DatabaseEntities.AnnualReportStatus.Saved;
                _repositoryWrapper.AnnualReports.Update(annualReport);
            }
        }

        private async Task ChangeCityAdministrationAsync(AnnualReport annualReport)
        {
            var oldCityAdministration = await _repositoryWrapper.CityAdministration
                .FindByCondition(ca => ca.CityId == annualReport.CityId && ca.EndDate == null && ca.AdminTypeId == _cityAdminType.ID)
                .FirstOrDefaultAsync();
            if (oldCityAdministration != null && oldCityAdministration.UserId != annualReport.CityManagement.UserId)
            {
                var user = await _userManager.FindByIdAsync(oldCityAdministration.UserId);
                await _userManager.RemoveFromRoleAsync(user, _cityAdminType.AdminTypeName);
                oldCityAdministration.EndDate = DateTime.Now;
                _repositoryWrapper.CityAdministration.Update(oldCityAdministration);
            }
            if ((oldCityAdministration == null || oldCityAdministration.UserId != annualReport.CityManagement.UserId) && annualReport.CityManagement.UserId != null)
            {
                var user = await _userManager.FindByIdAsync(annualReport.CityManagement.UserId);
                await _userManager.AddToRoleAsync(user, _cityAdminType.AdminTypeName);
                await _repositoryWrapper.CityAdministration.CreateAsync(new CityAdministration
                {
                    CityId = annualReport.CityId,
                    UserId = annualReport.CityManagement.UserId,
                    AdminTypeId = _cityAdminType.ID,
                    StartDate = DateTime.Now
                });
            }
            annualReport.CityManagement.CityAdminOldId = oldCityAdministration?.ID;
        }

        private async Task ChangeCityLegalStatusAsync(AnnualReport annualReport)
        {
            var oldCityLegalStatus = await _repositoryWrapper.CityLegalStatuses
                .FindByCondition(cls => cls.CityId == annualReport.CityId && cls.DateFinish == null)
                .FirstOrDefaultAsync();
            if (oldCityLegalStatus != null && oldCityLegalStatus.LegalStatusType != annualReport.CityManagement.CityLegalStatusNew)
            {
                oldCityLegalStatus.DateFinish = DateTime.Now;
                _repositoryWrapper.CityLegalStatuses.Update(oldCityLegalStatus);
            }
            if (oldCityLegalStatus == null || oldCityLegalStatus.LegalStatusType != annualReport.CityManagement.CityLegalStatusNew)
            {
                await _repositoryWrapper.CityLegalStatuses.CreateAsync(new CityLegalStatus
                {
                    CityId = annualReport.CityId,
                    LegalStatusType = annualReport.CityManagement.CityLegalStatusNew,
                    DateStart = DateTime.Now
                });
            }
            annualReport.CityManagement.CityLegalStatusOldId = oldCityLegalStatus?.Id;
        }

        private async Task RevertCityAdministrationAsync(int revertPointId, int cityId)
        {
            var cityAdministraions = await _repositoryWrapper.CityAdministration
                .FindByCondition(ca => ca.ID > revertPointId && ca.CityId == cityId && ca.AdminTypeId == _cityAdminType.ID)
                .ToListAsync();
            foreach (var cityAdministration in cityAdministraions)
            {
                var user = await _userManager.FindByIdAsync(cityAdministration.UserId);
                await _userManager.RemoveFromRoleAsync(user, _cityAdminType.AdminTypeName);
                _repositoryWrapper.CityAdministration.Delete(cityAdministration);
            }
            var cityAdministrationRevertPoint = await _repositoryWrapper.CityAdministration
                .FindByCondition(ca => ca.ID == revertPointId)
                .FirstOrDefaultAsync();
            if (cityAdministrationRevertPoint != null)
            {
                var user = await _userManager.FindByIdAsync(cityAdministrationRevertPoint.UserId);
                await _userManager.AddToRoleAsync(user, _cityAdminType.AdminTypeName);
                cityAdministrationRevertPoint.EndDate = null;
                _repositoryWrapper.CityAdministration.Update(cityAdministrationRevertPoint);
            }
        }

        private async Task RevertCityLegalStatusAsync(int revertPointId, int cityId)
        {
            var cityLegalStatuses = await _repositoryWrapper.CityLegalStatuses
                .FindByCondition(cls => cls.Id > revertPointId && cls.CityId == cityId)
                .ToListAsync();
            foreach (var cityLegalStatus in cityLegalStatuses)
            {
                _repositoryWrapper.CityLegalStatuses.Delete(cityLegalStatus);
            }
            var cityLegalStatusRevertPoint = await _repositoryWrapper.CityLegalStatuses
                .FindByCondition(cls => cls.Id == revertPointId)
                .FirstOrDefaultAsync();
            if (cityLegalStatusRevertPoint != null)
            {
                cityLegalStatusRevertPoint.DateFinish = null;
                _repositoryWrapper.CityLegalStatuses.Update(cityLegalStatusRevertPoint);
            }
        }
    }
}