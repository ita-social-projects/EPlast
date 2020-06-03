using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Services.Club
{
    public class ClubService : IClubService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IHostingEnvironment _env;
        private readonly IMapper _mapper;

        public ClubService(IRepositoryWrapper repoWrapper, IMapper mapper, IHostingEnvironment env)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _env = env;
        }

        public IEnumerable<ClubDTO> GetAllClubs()
        {
            var clubs = _repoWrapper.Club.FindAll().ToList();
            return _mapper.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubDTO>>(clubs);
        }

        public ClubProfileDTO GetClubProfile(int clubId)
        {
            var club = GetByIdWithDetails(clubId);
            var members = GetClubMembers(club, true, 6);
            var followers = GetClubMembers(club, false, 6);
            var clubAdmin = GetCurrentClubAdmin(club);
            var clubAdministration = GetCurrentClubAdministration(club);

            return new ClubProfileDTO { Club = club, Members = members, Followers = followers, ClubAdmin = clubAdmin, ClubAdministration = clubAdministration};
        }

        public ClubDTO GetClubInfoById(int id)
        {
            var club = _repoWrapper.Club
                .FindByCondition(q => q.ID == id)
                .FirstOrDefault();
            return _mapper.Map<DataAccessClub.Club, ClubDTO>(club);
        }

        private ClubDTO GetByIdWithDetails(int id)
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
                   .FirstOrDefault();
            return _mapper.Map<DataAccessClub.Club, ClubDTO>(club);
        }

        private UserDTO GetCurrentClubAdmin(ClubDTO club)
        {
            var clubAdmin = club.ClubAdministration
                    .Where(a => (a.EndDate >= DateTime.Now || a.EndDate == null) && a.AdminType.AdminTypeName == "Курінний")
                    .Select(a => a.ClubMembers.User)
                    .FirstOrDefault();
            return clubAdmin;
        }
        private IEnumerable<ClubAdministrationDTO> GetCurrentClubAdministration(ClubDTO club)
        {
            var clubAdministration = club.ClubAdministration
                .Where(a => a.EndDate >= DateTime.Now || a.EndDate == null)
                .ToList();
            return clubAdministration;
        }

        private List<ClubMembersDTO> GetClubMembers(ClubDTO club, bool isApproved, int amount)
        {
            var members = club.ClubMembers.Where(m => m.IsApproved == isApproved)
                .Take(amount)
                .ToList();
            return members;
        }

        private List<ClubMembersDTO> GetClubMembers(ClubDTO club, bool isApproved)
        {
            var members = club.ClubMembers.Where(m => m.IsApproved == isApproved)
                .ToList();
            return members;
        }

        public void Update(ClubDTO club, IFormFile file)
        {
            var oldImageName = GetClubInfoById(club.ID).Logo;
            UpdateOrCreateAnImage(club, file, oldImageName);
            _repoWrapper.Club.Update(_mapper.Map<ClubDTO, DataAccessClub.Club>(club));
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
            var newClub = _mapper.Map<ClubDTO, DataAccessClub.Club>(club);
            UpdateOrCreateAnImage(club, file);
            _repoWrapper.Club.Create(newClub);
            _repoWrapper.Save();
            return _mapper.Map<DataAccessClub.Club, ClubDTO>(newClub);
        }

        public ClubProfileDTO GetClubMembersOrFollowers(int clubId, bool isApproved)
        {
            var club = GetByIdWithDetails(clubId);
            var members = GetClubMembers(club, isApproved);
            var clubAdmin = GetCurrentClubAdmin(club);

            return isApproved
                ? new ClubProfileDTO { Club = club, ClubAdmin = clubAdmin, Members = members }
                : new ClubProfileDTO { Club = club, ClubAdmin = clubAdmin, Followers = members };
        }
    }
}
