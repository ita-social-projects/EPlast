using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Club
{
    public class ClubAnnualReportService: IClubAnnualReportService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IClubAccessService _clubAccessService;
        private readonly IMapper _mapper;

        public ClubAnnualReportService(IRepositoryWrapper repositoryWrapper,
                                    IClubAccessService clubAccessService, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _clubAccessService = clubAccessService;
            _mapper = mapper;
        }

        ///<inheritdoc/>

        public async Task<ClubAnnualReportDTO> GetByIdAsync(ClaimsPrincipal claimsPrincipal, int id)
        {
            var clubAnnualReport = await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(
                    predicate: a => a.ID == id,
                    include: source => source
                        .Include(a => a.Club)
                            .ThenInclude(c => c.ClubAdministration)
                                .ThenInclude(cb=>cb.AdminType)
                        .Include(ad=>ad.Club)
                            .ThenInclude(ac=>ac.ClubAdministration)
                                .ThenInclude(ar=>ar.User)
                        .Include(ca => ca.Club)
                            .ThenInclude(cm => cm.ClubMembers)
                                .ThenInclude(mc=>mc.User));
            return await _clubAccessService.HasAccessAsync(claimsPrincipal, clubAnnualReport.ClubId) ? _mapper.Map<ClubAnnualReport, ClubAnnualReportDTO>(clubAnnualReport)
                : throw new UnauthorizedAccessException();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<ClubAnnualReportDTO>> GetAllAsync(ClaimsPrincipal claimsPrincipal)
        {
            var annualReports = await _repositoryWrapper.ClubAnnualReports.GetAllAsync(
                    include: source => source
                        .Include(ar => ar.Club)
                            .ThenInclude(c=>c.ClubAdministration)
                        .Include(ca=>ca.Club)
                            .ThenInclude(cm=>cm.ClubMembers));
            return _mapper.Map<IEnumerable<ClubAnnualReport>, IEnumerable<ClubAnnualReportDTO>>(annualReports);
        }

        public async Task CreateAsync(ClaimsPrincipal claimsPrincipal, ClubAnnualReportDTO clubAnnualReportDTO)
        {
            var club = await _repositoryWrapper.Club.GetFirstOrDefaultAsync(
                predicate: a => a.ID == clubAnnualReportDTO.ClubId,
                include:source=>source
                .Include(a=>a.ClubMembers)
                    .ThenInclude(ac=>ac.User)
                        .ThenInclude(af=>af.UserPlastDegrees)
                            .ThenInclude(ad=>ad.PlastDegree)
                .Include(a=>a.ClubAdministration)
                    .ThenInclude(ap=>ap.User)
                        .ThenInclude(af => af.UserPlastDegrees)
                            .ThenInclude(ad => ad.PlastDegree)

                );
            if (await CheckCreated(club.ID))
            {
                throw new InvalidOperationException();
            }
            if (!await _clubAccessService.HasAccessAsync(claimsPrincipal, club.ID))
            {
                throw new UnauthorizedAccessException();
            }
            foreach (var item in club.ClubMembers)
            {
                var cityMember = await _repositoryWrapper.CityMembers.GetFirstOrDefaultAsync(predicate: a => a.UserId == item.UserId,include:source=>source.Include(ar=>ar.City));
                clubAnnualReportDTO.ClubMembersSummary += $"{item.User.UserPlastDegrees.FirstOrDefault(x=>x.UserId==item.User.Id).PlastDegree.Name}, " +
                                                            $"{item.User.FirstName} {item.User.LastName}," +
                                                            $"{item.IsApproved},"+
                                                            $"{cityMember.City.Name};";
            }
            foreach(var item in club.ClubAdministration)
            {

                clubAnnualReportDTO.ClubAdminContacts += $"{item.User.UserPlastDegrees.FirstOrDefault(x=>x.UserId==item.User.Id).PlastDegree.Name}," +
                                                            $"{item.User.FirstName} {item.User.LastName}," +
                                                            $"{item.User.Email}," +
                                                            $"{item.User.PhoneNumber}";
            }
            var clubAnnualReport = _mapper.Map<ClubAnnualReportDTO, ClubAnnualReport>(clubAnnualReportDTO);

            await _repositoryWrapper.ClubAnnualReports.CreateAsync(clubAnnualReport);
            await _repositoryWrapper.SaveAsync();

        }

        private async Task<bool> CheckCreated(int Id)
        {
            return await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.Club.ID == Id && a.Date.Year == DateTime.Now.Year) != null;
        }
    }
}
