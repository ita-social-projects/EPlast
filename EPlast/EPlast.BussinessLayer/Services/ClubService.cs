using EPlast.BussinessLayer.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using EPlast.BussinessLayer.DTO;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;

namespace EPlast.BussinessLayer.Services
{
    public class ClubService : IClubService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IHostingEnvironment _env;
        private UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public ClubService(IRepositoryWrapper repoWrapper, IMapper mapper, IHostingEnvironment env, UserManager<User> userManager)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _env = env;
            _userManager = userManager;
        }

        public IEnumerable<ClubDTO> GetAllClubs()
        {
            var clubs = _repoWrapper.Club.FindAll().ToList();
            return _mapper.Map<IEnumerable<Club>, IEnumerable<ClubDTO>>(clubs);
        }

        public ClubProfileDTO GetClubProfile(int clubId)
        {
            var club = GetByIdWithDetails(clubId);
            var members = GetClubMembers(club, true, 6);
            var followers = GetClubMembers(club, false, 6);
            var clubAdmin = GetCurrentClubAdmin(club);

            return new ClubProfileDTO { Club = club, Members = members, Followers = followers, ClubAdmin = clubAdmin };
        }

        public ClubDTO GetById(int id)
        {
            var club = _repoWrapper.Club
                .FindByCondition(q => q.ID == id)
                .FirstOrDefault();
            return _mapper.Map<Club, ClubDTO>(club);
        }

        public ClubDTO GetByIdWithDetails(int id)
        {
            var club = _repoWrapper.Club
                   .FindByCondition(q => q.ID == id)
                   .Include(c => c.ClubAdministration)
                   .ThenInclude(t => t.AdminType)
                   .Include(n => n.ClubAdministration)
                   .ThenInclude(t => t.ClubMembers)
                   .ThenInclude(us => us.User)
                   .Include(m => m.ClubMembers)
                   .ThenInclude(u => u.User)
                   .FirstOrDefault()
                   ;
            return _mapper.Map<Club, ClubDTO>(club);
        }

        public UserDTO GetCurrentClubAdmin(ClubDTO club)
        {
            var clubAdmin = club.ClubAdministration
                    .Where(a => (a.EndDate >= DateTime.Now || a.EndDate == null) && a.AdminType.AdminTypeName == "Курінний")
                    .Select(a => a.ClubMembers.User)
                    .FirstOrDefault();
            return clubAdmin;
        }

        public List<ClubMembersDTO> GetClubMembers(ClubDTO club, bool isApproved, int amount)
        {
            var members = club.ClubMembers.Where(m => m.IsApproved == isApproved)
                .Take(amount)
                .ToList();
            return members;
        }

        public List<ClubMembersDTO> GetClubMembers(ClubDTO club, bool isApproved)
        {
            var members = club.ClubMembers.Where(m => m.IsApproved == isApproved)
                .ToList();
            return members;
        }

        public ClubDTO GetClubAdministration(int clubID)
        {
            var club = _repoWrapper.Club
                .FindByCondition(q => q.ID == clubID)
                .Include(c => c.ClubAdministration)
                .ThenInclude(t => t.AdminType)
                .Include(n => n.ClubAdministration)
                .ThenInclude(t => t.ClubMembers)
                .ThenInclude(us => us.User)
                .FirstOrDefault();
            return _mapper.Map<Club, ClubDTO>(club);
        }

        public ClubProfileDTO GetCurrentClubAdminByID(int clubID)
        {
            var club = GetClubAdministration(clubID);
            return new ClubProfileDTO { Club = club, ClubAdministration = club.ClubAdministration };
        }

        public ClubProfileDTO GetClubMembersOrFollowers(int clubId, bool isApproved)
        {
            var club = GetByIdWithDetails(clubId);
            var members = GetClubMembers(club, isApproved, 6);
            var clubAdmin = GetCurrentClubAdmin(club);

            return isApproved ? new ClubProfileDTO { Club = club, ClubAdmin = clubAdmin, Members = members } :
                                new ClubProfileDTO { Club = club, ClubAdmin = clubAdmin, Followers = members };
        }

        public void Update(ClubDTO club, IFormFile file)
        {
            var oldImageName = GetById(club.ID).Logo;
            UpdateOrCreateAnImage(club, file, oldImageName);
            _repoWrapper.Club.Update(_mapper.Map<ClubDTO, Club>(club));
            _repoWrapper.Save();
        }

        private void UpdateOrCreateAnImage(ClubDTO club, IFormFile file, string oldImageName = null)
        {
            if (file != null && file.Length > 0)
            {
                var img = Image.FromStream(file.OpenReadStream());
                var uploads = Path.Combine(_env.WebRootPath, "images\\Club");

                if (!string.IsNullOrEmpty(oldImageName))
                {
                    var oldPath = Path.Combine(uploads, oldImageName);

                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }

                }

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, fileName);
                img.Save(filePath);
                club.Logo = fileName;
            }
            else
            {
                club.Logo = oldImageName;
            }
        }
        public ClubDTO Create(ClubDTO club, IFormFile file)
        {
            var newClub = _mapper.Map<ClubDTO, Club>(club);
                UpdateOrCreateAnImage(club, file);
            _repoWrapper.Club.Create(newClub);
            _repoWrapper.Save();
            return _mapper.Map<Club, ClubDTO>(newClub);
        }

        public void ToggleIsApprovedInClubMembers(int memberId, int clubId)
        {
            var club = GetByIdWithDetails(clubId);
            var person = _repoWrapper.ClubMembers
                .FindByCondition(u => u.ID == memberId)
                .FirstOrDefault();

            if (person != null)
            {
                person.IsApproved = !person.IsApproved;
            }

            _repoWrapper.ClubMembers.Update(person);
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

        public void SetAdminEndDate(AdminEndDateDTO adminEndDate)
        {
            ClubAdministration admin = _repoWrapper.GetClubAdministration
                .FindByCondition(i => i.ID == adminEndDate.adminId)
                .FirstOrDefault();
            admin.EndDate = adminEndDate.enddate;
            _repoWrapper.GetClubAdministration.Update(admin);
            _repoWrapper.Save();
        }

        public void AddClubAdmin(ClubAdministrationDTO createdAdmin)
        {
            var adminType = _repoWrapper.AdminType
                    .FindByCondition(i => i.AdminTypeName == createdAdmin.AdminTypeName)
                    .FirstOrDefault();
            int AdminTypeId;

            if (adminType == null)
            {
                var newAdminType = new AdminType() { AdminTypeName = createdAdmin.AdminTypeName };
                _repoWrapper.AdminType.Create(newAdminType);
                _repoWrapper.Save();
                AdminTypeId = newAdminType.ID;
            }
            else
            {
                AdminTypeId = adminType.ID;
            }

            ClubAdministration newClubAdmin = new ClubAdministration()
            {
                ClubMembersID = createdAdmin.ClubMembersID,
                StartDate = createdAdmin.StartDate,
                EndDate = createdAdmin.EndDate,
                ClubId = createdAdmin.ClubId,
                AdminTypeId = AdminTypeId
            };

            _repoWrapper.GetClubAdministration.Create(newClubAdmin);
            _repoWrapper.Save();
        }

        public void AddFollower(int index, string userId)
        {
            ClubMembers oldMember =
                _repoWrapper.ClubMembers
                .FindByCondition(i => i.UserId == userId)
                .FirstOrDefault();

            if (oldMember != null)
            {
                _repoWrapper.ClubMembers.Delete(oldMember);
                _repoWrapper.Save();
            }

            ClubMembers newMember = new ClubMembers()
            {
                ClubId = index,
                IsApproved = false,
                UserId = userId
            };

            _repoWrapper.ClubMembers.Create(newMember);
            _repoWrapper.Save();
        }
    }
}
