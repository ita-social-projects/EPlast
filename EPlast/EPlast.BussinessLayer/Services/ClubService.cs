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
using Microsoft.AspNetCore.Hosting;
using EPlast.DataAccess.DTO;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BussinessLayer.Services
{
    public class ClubService : IClubService
    {
        private readonly IRepositoryWrapper repoWrapper;
        private readonly IHostingEnvironment env;
        private readonly UserManager<User> userManager;

        public ClubService(IRepositoryWrapper repoWrapper, IHostingEnvironment env, UserManager<User> userManager)
        {
            this.repoWrapper = repoWrapper;
            this.env = env;
            this.userManager = userManager;
        }
        public List<Club> GetAllClubs()
        {
            return repoWrapper.Club.FindAll().ToList();
        }

        public Club GetByIdWithDetails(int id)
        {
            var club = repoWrapper.Club
                   .FindByCondition(q => q.ID == id)
                   .Include(c => c.ClubAdministration)
                   .ThenInclude(t => t.AdminType)
                   .Include(n => n.ClubAdministration)
                   .ThenInclude(t => t.ClubMembers)
                   .ThenInclude(us => us.User)
                   .Include(m => m.ClubMembers)
                   .ThenInclude(u => u.User)
                   .FirstOrDefault();
            return club;
        }

        public User GetCurrentClubAdmin(Club club)
        {
            var clubAdmin = club.ClubAdministration
                    .Where(a => (a.EndDate >= DateTime.Now || a.EndDate == null) && a.AdminType.AdminTypeName == "Курінний")
                    .Select(a => a.ClubMembers.User)
                    .FirstOrDefault();
            return clubAdmin;
        }

        public List<ClubMembers> GetClubMembers(Club club, bool isApproved, int amount)
        {
            var members = club.ClubMembers.Where(m => m.IsApproved == isApproved)
                .Take(amount)
                .ToList();
            return members;
        }

        public List<ClubMembers> GetClubMembers(Club club, bool isApproved)
        {
            var members = club.ClubMembers.Where(m => m.IsApproved == isApproved)
                .ToList();
            return members;
        }

        public Club GetById(int id)
        {
            var club = repoWrapper.Club
                   .FindByCondition(q => q.ID == id)
                   .FirstOrDefault();
            return club;
        }

        public void Update(Club club, IFormFile file)
        {
            var oldImageName = GetById(club.ID).Logo;
            UpdateOrCreateAnImage(club, file, oldImageName);
            repoWrapper.Club.Update(club);
            repoWrapper.Save();
        }
        private void UpdateOrCreateAnImage(Club club, IFormFile file, string oldImageName = null)
        {
            if (file != null && file.Length > 0)
            {
                var img = Image.FromStream(file.OpenReadStream());
                var uploads = Path.Combine(env.WebRootPath, "images\\Club");

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
        public void Create(Club club, IFormFile file)
        {
            UpdateOrCreateAnImage(club, file);
            repoWrapper.Club.Create(club);
            repoWrapper.Save();
        }

        public void ToggleIsApprovedInClubMembers(int memberId, int clubId)
        {
            var club = GetByIdWithDetails(clubId);
            var person = repoWrapper.ClubMembers
                .FindByCondition(u => u.ID == memberId)
                .FirstOrDefault();

            if (person != null)
            {
                person.IsApproved = !person.IsApproved;
            }

            repoWrapper.ClubMembers.Update(person);
            repoWrapper.Save();
        }

        public bool DeleteClubAdmin(int id)
        {
            ClubAdministration admin = repoWrapper.GetClubAdministration
                .FindByCondition(i => i.ID == id).FirstOrDefault();
            if (admin != null)
            {
                repoWrapper.GetClubAdministration.Delete(admin);
                repoWrapper.Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetAdminEndDate(AdminEndDateDTO adminEndDate)
        {
            ClubAdministration admin = repoWrapper.GetClubAdministration
                .FindByCondition(i => i.ID == adminEndDate.adminId)
                .FirstOrDefault();
            admin.EndDate = adminEndDate.enddate;
            repoWrapper.GetClubAdministration.Update(admin);
            repoWrapper.Save();
        }

        public void AddClubAdmin(ClubAdministrationDTO createdAdmin)
        {
            var adminType = repoWrapper.AdminType
                    .FindByCondition(i => i.AdminTypeName == createdAdmin.AdminType)
                    .FirstOrDefault();
            int AdminTypeId;

            if (adminType == null)
            {
                var newAdminType = new AdminType() { AdminTypeName = createdAdmin.AdminType };
                repoWrapper.AdminType.Create(newAdminType);
                repoWrapper.Save();
                adminType = repoWrapper.AdminType
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

            repoWrapper.GetClubAdministration.Create(newClubAdmin);
            repoWrapper.Save();
        }

        public void AddFollower(int index, string userId)
        {
            ClubMembers oldMember =
                repoWrapper.ClubMembers
                .FindByCondition(i => i.UserId == userId)
                .FirstOrDefault();

            if (oldMember != null)
            {
                repoWrapper.ClubMembers.Delete(oldMember);
                repoWrapper.Save();
            }

            ClubMembers newMember = new ClubMembers()
            {
                ClubId = index,
                IsApproved = false,
                UserId = userId
            };

            repoWrapper.ClubMembers.Create(newMember);
            repoWrapper.Save();
        }
    }
}
