using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.DTO;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public void AddClubAdmin(ClubAdministrationDTO createdAdmin)
        {
            var adminType = _repoWrapper.AdminType
                    .FindByCondition(i => i.AdminTypeName == createdAdmin.AdminType)
                    .FirstOrDefault();
            int AdminTypeId;

            if (adminType == null)
            {
                var newAdminType = new AdminType() { AdminTypeName = createdAdmin.AdminType };
                _repoWrapper.AdminType.Create(newAdminType);
                _repoWrapper.Save();
                adminType = _repoWrapper.AdminType
                    .FindByCondition(i => i.AdminTypeName == createdAdmin.AdminType)
                    .FirstOrDefault();
                AdminTypeId = adminType.ID;
            }
            else
            {
                AdminTypeId = adminType.ID;
            }

            ClubAdministration newClubAdmin = new ClubAdministration()
            {
                ClubMembersID = createdAdmin.adminId,
                StartDate = createdAdmin.startdate,
                EndDate = createdAdmin.enddate,
                ClubId = createdAdmin.clubIndex,
                AdminTypeId = AdminTypeId
            };

            _repoWrapper.GetClubAdministration.Create(newClubAdmin);
            _repoWrapper.Save();
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

        public UserDTO GetCurrentClubAdmin(ClubDTO club)
        {
            var clubAdmin = club.ClubAdministration
                    .Where(a => (a.EndDate >= DateTime.Now || a.EndDate == null) && a.AdminType.AdminTypeName == "Курінний")
                    .Select(a => a.ClubMembers.User)
                    .FirstOrDefault();
            return clubAdmin;
        }

        public void SetAdminEndDate(AdminEndDateDTO adminEndDate)
        {
            ClubAdministration admin = _repoWrapper.GetClubAdministration
                .FindByCondition(i => i.ID == adminEndDate.adminId)
                .FirstOrDefault();
            admin.EndDate = adminEndDate.enddate;
            _repoWrapper.GetClubAdministration.Update(admin);
            _repoWrapper.Save();
        }
    }
}
