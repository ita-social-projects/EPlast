using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EPlast.BussinessLayer.Services.Club
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
        private ClubDTO GetClubAdministration(int clubID)
        {
            var club = _repoWrapper.Club
                .FindByCondition(q => q.ID == clubID)
                .Include(c => c.ClubAdministration)
                .ThenInclude(t => t.AdminType)
                .Include(n => n.ClubAdministration)
                .ThenInclude(t => t.ClubMembers)
                .ThenInclude(us => us.User)
                .FirstOrDefault();
            return _mapper.Map<DataAccess.Entities.Club, ClubDTO>(club);
        }

        public ClubProfileDTO GetCurrentClubAdministrationByID(int clubID)
        {
            var club = GetClubAdministration(clubID);
            return new ClubProfileDTO { Club = club, ClubAdministration = club.ClubAdministration };
        }

        public bool DeleteClubAdmin(int id)
        {
            ClubAdministration admin = _repoWrapper.GetClubAdministration
                .FindByCondition(i => i.ID == id).FirstOrDefault();
            if (admin != null)
            {
                _repoWrapper.GetClubAdministration.Delete(admin);
                _repoWrapper.Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetAdminEndDate(AdminEndDateDTO adminEndDate)
        {
            ClubAdministration admin = _repoWrapper.GetClubAdministration
                .FindByCondition(i => i.ID == adminEndDate.AdminId)
                .FirstOrDefault();
            admin.EndDate = adminEndDate.EndDate;
            _repoWrapper.GetClubAdministration.Update(admin);
            _repoWrapper.Save();
        }

        public void AddClubAdmin(ClubAdministrationDTO createdAdmin)
        {
            var adminType = _repoWrapper.AdminType
                .FindByCondition(i => i.AdminTypeName == createdAdmin.AdminTypeName)
                .FirstOrDefault();
            int adminTypeId;

            if (adminType == null)
            {
                var newAdminType = new AdminType() { AdminTypeName = createdAdmin.AdminTypeName };
                _repoWrapper.AdminType.Create(newAdminType);
                _repoWrapper.Save();
                adminTypeId = newAdminType.ID;
            }
            else
            {
                adminTypeId = adminType.ID;
            }

            ClubAdministration newClubAdmin = new ClubAdministration()
            {
                ClubMembersID = createdAdmin.ClubMembersID,
                StartDate = createdAdmin.StartDate,
                EndDate = createdAdmin.EndDate,
                ClubId = createdAdmin.ClubId,
                AdminTypeId = adminTypeId
            };

            _repoWrapper.GetClubAdministration.Create(newClubAdmin);
            _repoWrapper.Save();
        }
    }
}
