﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;


namespace EPlast.BLL.Services
{
    public class AnnualReportService : IAnnualReportService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ICityAccessService _cityAccessService;
        private readonly IRegionAnnualReportService _regionAnnualReportService;
        private readonly IMapper _mapper;

        public AnnualReportService(IRepositoryWrapper repositoryWrapper, ICityAccessService cityAccessService, IRegionAnnualReportService regionAnnualReportService,
        IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _cityAccessService = cityAccessService;
            _regionAnnualReportService = regionAnnualReportService;
            _mapper = mapper;
        }

        ///<inheritdoc/>
        public async Task<AnnualReportDto> GetByIdAsync(User user, int id)
        {
            var annualReport = await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(
                    predicate: a => a.ID == id,
                    include: source => source
                        .Include(a => a.NewCityAdmin)
                        .Include(a => a.MembersStatistic)
                        .Include(a => a.City));
            return await _cityAccessService.HasAccessAsync(user, annualReport.CityId) ? _mapper.Map<AnnualReport, AnnualReportDto>(annualReport)
                : throw new UnauthorizedAccessException();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<AnnualReportDto>> GetAllAsync(User user)
        {
            var annualReports = await _repositoryWrapper.AnnualReports.GetAllAsync(
                    include: source => source
                        .Include(ar => ar.Creator)
                        .Include(ar => ar.City)
                            .ThenInclude(c => c.Region));
            var citiesDto = await _cityAccessService.GetCitiesAsync(user);
            var filteredAnnualReports = annualReports.Where(ar => citiesDto.Any(c => c.ID == ar.CityId));
            return _mapper.Map<IEnumerable<AnnualReport>, IEnumerable<AnnualReportDto>>(filteredAnnualReports);
        }
        
        ///<inheritdoc/>
        public async Task<IEnumerable<AnnualReportTableObject>> GetAllAsync(User user, bool isAdmin, string searchedData, int page, int pageSize, int sortKey, bool auth)
        {
            return await _repositoryWrapper.AnnualReports.GetAnnualReportsAsync(user.Id, isAdmin, searchedData, page,
                pageSize, sortKey, auth);
        }

        /// <inheritdoc />
        public async Task<CityDto> GetCityMembersAsync(int cityId)
        {
            var city = await _repositoryWrapper.City.GetFirstOrDefaultAsync(
                predicate: c => c.ID == cityId,
                include: source => source
                    .Include(m => m.CityMembers)
                    .ThenInclude(u => u.User));
            if (city == null)
            {
                return null;
            }

            city.CityMembers = city.CityMembers
                .Where(m => m.IsApproved)
                .ToList();

            return _mapper.Map<DataAccess.Entities.City, CityDto>(city);
        }

        ///<inheritdoc/>
        public async Task CreateAsync(User user, AnnualReportDto annualReportDTO)
        {
            var city = await _repositoryWrapper.City.GetFirstOrDefaultAsync(
                predicate: a => a.ID == annualReportDTO.CityId);
            if (!await _cityAccessService.HasAccessAsync(user, city.ID))
            {
                throw new UnauthorizedAccessException();
            }
            if (await CheckCreated(city.ID))
            {
                throw new InvalidOperationException();
            }
            var annualReport = _mapper.Map<AnnualReportDto, AnnualReport>(annualReportDTO);
            annualReport.CreatorId = user.Id;
            annualReport.CreatorFirstName = user.FirstName;
            annualReport.CreatorLastName = user.LastName;
            annualReport.CreatorFatherName = user.FatherName;
            annualReport.Date = DateTime.Now;
            annualReport.Status = AnnualReportStatus.Unconfirmed;
            await _repositoryWrapper.AnnualReports.CreateAsync(annualReport);
            await _repositoryWrapper.SaveAsync();
        }

        ///<inheritdoc/>
        public async Task EditAsync(User user, AnnualReportDto annualReportDTO)
        {
            var annualReport = await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(
                    predicate: a => a.ID == annualReportDTO.ID && a.CityId == annualReportDTO.CityId && a.CreatorId == annualReportDTO.CreatorId
                        && a.Date.Date == annualReportDTO.Date.Date && a.Status == AnnualReportStatus.Unconfirmed);
            if (annualReportDTO.Status != AnnualReportStatusDto.Unconfirmed)
            {
                throw new InvalidOperationException();
            }
            if (!await _cityAccessService.HasAccessAsync(user, annualReport.CityId))
            {
                throw new UnauthorizedAccessException();
            }
            annualReport = _mapper.Map<AnnualReportDto, AnnualReport>(annualReportDTO);
            _repositoryWrapper.AnnualReports.Update(annualReport);
            await _repositoryWrapper.SaveAsync();
        }

        ///<inheritdoc/>
        public async Task ConfirmAsync(User user, int id)
        {
            var annualReport = await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(
                    predicate: a => a.ID == id && a.Status == AnnualReportStatus.Unconfirmed,
                    include: source => source
                        .Include(a => a.City));
            annualReport.Status = AnnualReportStatus.Confirmed;
            _repositoryWrapper.AnnualReports.Update(annualReport);
            await SaveLastConfirmedAsync(annualReport.CityId);
            await _repositoryWrapper.SaveAsync();
            await _regionAnnualReportService.UpdateMembersInfo(annualReport.City.RegionId, annualReport.Date.Year);
        }

        ///<inheritdoc/>
        public async Task CancelAsync(User user, int id)
        {
            var annualReport = await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(
                    predicate: a => a.ID == id && a.Status == AnnualReportStatus.Confirmed,
                    include: source => source
                        .Include(a => a.City));
            annualReport.Status = AnnualReportStatus.Unconfirmed;
            _repositoryWrapper.AnnualReports.Update(annualReport);
            await _repositoryWrapper.SaveAsync();
            await _regionAnnualReportService.UpdateMembersInfo(annualReport.City.RegionId, annualReport.Date.Year);
        }

        ///<inheritdoc/>
        public async Task DeleteAsync(User user, int id)
        {
            var annualReport = await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(
                    predicate: a => a.ID == id && a.Status == AnnualReportStatus.Unconfirmed,
                    include: source => source
                        .Include(a => a.City));
            await _regionAnnualReportService.UpdateMembersInfo(annualReport.City.RegionId, annualReport.Date.Year);
            _repositoryWrapper.AnnualReports.Delete(annualReport);
            await _repositoryWrapper.SaveAsync();
        }

        ///<inheritdoc/>
        public async Task<bool> CheckCreated(User user, int cityId)
        {
            if (!await _cityAccessService.HasAccessAsync(user, cityId))
            {
                throw new UnauthorizedAccessException();
            }
            return await CheckCreated(cityId);
        }

        private async Task SaveLastConfirmedAsync(int cityId)
        {
            var annualReport = await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.CityId == cityId && a.Status == AnnualReportStatus.Confirmed);
            if (annualReport != null)
            {
                annualReport.Status = AnnualReportStatus.Saved;
                _repositoryWrapper.AnnualReports.Update(annualReport);
            }
        }

        private async Task<bool> CheckCreated(int cityId)
        {
            return await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.CityId == cityId && (a.Date.Year == DateTime.Now.Year)) != null;
        }

        public async Task<AnnualReportDto> GetEditFormByIdAsync(User user, int id)
        {
            var annualReport = await _repositoryWrapper.AnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.ID == id && a.Status==AnnualReportStatus.Unconfirmed,
                include: source => source
                    .Include(a => a.NewCityAdmin)
                    .Include(a => a.MembersStatistic)
                    .Include(a => a.City.CityMembers)
                    .ThenInclude(m=>m.User));
            return (await _cityAccessService.HasAccessAsync(user, annualReport.CityId)) ? _mapper.Map<AnnualReport, AnnualReportDto>(annualReport)
                : throw new UnauthorizedAccessException();
        }
    }
}
