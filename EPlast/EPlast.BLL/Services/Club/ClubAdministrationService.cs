using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Club
{
    public class ClubAdministrationService : IClubAdministrationService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public ClubAdministrationService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        private async Task<ClubDTO> GetClubAdministrationAsync(int clubID)
        {
            var club = await _repoWrapper.Club
                .GetFirstOrDefaultAsync(
                    i => i.ID == clubID,
                    i => i
                        .Include(c => c.ClubAdministration)
                            .ThenInclude(t => t.AdminType)
                        .Include(n => n.ClubAdministration)
                            .ThenInclude(t => t.ClubMembers)
                            .ThenInclude(us => us.User));

            return _mapper.Map<DataAccess.Entities.Club, ClubDTO>(club);
        }

        public async Task<ClubProfileDTO> GetCurrentClubAdministrationByIDAsync(int clubID)
        {
            var club = await GetClubAdministrationAsync(clubID);
            
            var clubProfileDTO = new ClubProfileDTO
            {
                Club = club,
                ClubAdministration = club.ClubAdministration
            };

            return clubProfileDTO;
        }

        public async Task<bool> DeleteClubAdminAsync(int id)
        {
            var admin = await _repoWrapper.GetClubAdministration
                .GetFirstOrDefaultAsync(i => i.ID == id);

            if (admin != null)
            {
                _repoWrapper.GetClubAdministration.Delete(admin);
                await _repoWrapper.SaveAsync();
              
                return true;
            }
            
            return false;
        }

        public async Task SetAdminEndDateAsync(AdminEndDateDTO adminEndDate)
        {
            var admin = await _repoWrapper.GetClubAdministration
                .GetFirstOrDefaultAsync(i => i.ID == adminEndDate.AdminId);

            admin.EndDate = adminEndDate.EndDate;
            _repoWrapper.GetClubAdministration.Update(admin);
            
            await _repoWrapper.SaveAsync();
        }

        public async Task AddClubAdminAsync(ClubAdministrationDTO createdAdmin)
        {
            var adminType = await _repoWrapper.AdminType
                .GetFirstOrDefaultAsync(i => i.AdminTypeName == createdAdmin.AdminTypeName);

            int adminTypeId;

            if (adminType == null)
            {
                var newAdminType = new AdminType() { AdminTypeName = createdAdmin.AdminTypeName };
                adminTypeId = newAdminType.ID;
                
                await _repoWrapper.AdminType.CreateAsync(newAdminType);
                await _repoWrapper.SaveAsync();
            }
            else
                adminTypeId = adminType.ID;
            
            ClubAdministration newClubAdmin = new ClubAdministration()
            {
                ClubMembersID = createdAdmin.ClubMembersID,
                StartDate = createdAdmin.StartDate,
                EndDate = createdAdmin.EndDate,
                ClubId = createdAdmin.ClubId,
                AdminTypeId = adminTypeId
            };

            await _repoWrapper.GetClubAdministration.CreateAsync(newClubAdmin);
            await _repoWrapper.SaveAsync();
        }
    }
}
