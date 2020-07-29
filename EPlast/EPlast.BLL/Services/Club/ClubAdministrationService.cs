using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Club
{
    public class ClubAdministrationService : IClubAdministrationService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IAdminTypeService _adminTypeService;

        public ClubAdministrationService(IRepositoryWrapper repoWrapper, IMapper mapper,
            IAdminTypeService adminTypeService)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _adminTypeService = adminTypeService;
        }

        private async Task<ClubDTO> GetClubAdministrationAsync(int clubId)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(i => i.ID == clubId,
                i => i.Include(c => c.ClubAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(n => n.ClubAdministration)
                    .ThenInclude(t => t.ClubMembers)
                    .ThenInclude(us => us.User));

            return (club != null)
                ? _mapper.Map<DataAccess.Entities.Club, ClubDTO>(club)
                : throw new ArgumentNullException($"Club with {clubId} id not found");
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubAdministrationByIdAsync(int clubId)
        {
            var clubDto = await GetClubAdministrationAsync(clubId);

            return new ClubProfileDTO { Club = clubDto, ClubAdministration = clubDto.ClubAdministration };
        }

        /// <inheritdoc />
        public async Task<bool> DeleteClubAdminAsync(int id)
        {
            var clubAdministration = await _repoWrapper.ClubAdministration.GetFirstOrDefaultAsync(i => i.ID == id);

            if (clubAdministration == null)
            {
                throw new ArgumentNullException($"ClubAdministration with {id} ID not found");
            }

            _repoWrapper.ClubAdministration.Delete(clubAdministration);
            await _repoWrapper.SaveAsync();

            return true;
        }

        /// <inheritdoc />
        public async Task<ClubAdministrationDTO> SetAdminEndDateAsync(int clubAdministrationId, DateTime endDate)
        {
            var clubAdministration =
                await _repoWrapper.ClubAdministration.GetFirstOrDefaultAsync(i => i.ID == clubAdministrationId);

            if (clubAdministration == null)
            {
                throw new ArgumentNullException($"ClubAdministration with {clubAdministrationId} ID not found");
            }

            clubAdministration.EndDate = endDate;

            _repoWrapper.ClubAdministration.Update(clubAdministration);
            await _repoWrapper.SaveAsync();

            return _mapper.Map<ClubAdministration, ClubAdministrationDTO>(clubAdministration);
        }

        /// <inheritdoc />
        public async Task<ClubAdministrationDTO> AddClubAdminAsync(ClubAdministrationDTO createdAdmin)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(createdAdmin.AdminTypeName);
            createdAdmin.AdminTypeId = adminType.ID;

            ClubAdministration newClubAdmin = _mapper.Map<ClubAdministrationDTO, ClubAdministration>(createdAdmin);
            await _repoWrapper.ClubAdministration.CreateAsync(newClubAdmin);
            await _repoWrapper.SaveAsync();

            return _mapper.Map<ClubAdministration, ClubAdministrationDTO>(newClubAdmin);
        }
    }
}