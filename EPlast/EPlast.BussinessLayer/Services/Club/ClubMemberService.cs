using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPlast.BussinessLayer.Services.Club
{
    public class ClubMemberService : IClubMembersService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _env;

        public ClubMemberService(IRepositoryWrapper repoWrapper, IMapper mapper, IHostingEnvironment env)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _env = env;
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

        public IEnumerable<ClubMembers> GetClubMembers(DataAccess.Entities.Club club, bool isApproved, int amount)
        {
            var members = club.ClubMembers.Where(m => m.IsApproved == isApproved)
                .Take(amount)
                .ToList();
            return members;
        }

        public IEnumerable<ClubMembers> GetClubMembers(DataAccess.Entities.Club club, bool isApproved)
        {
            var members = club.ClubMembers.Where(m => m.IsApproved == isApproved)
                .ToList();
            return members;
        }

        public IEnumerable<ClubMembersDTO> GetClubMembersDTO(ClubDTO club, bool isApproved, int amount)
        {
           var tempClub = _repoWrapper.Club
                .FindByCondition(i => i.ID == club.ID)
                .FirstOrDefault();
            return _mapper.Map<IEnumerable<DataAccess.Entities.ClubMembers>, IEnumerable<ClubMembersDTO>>(GetClubMembers(tempClub, isApproved, amount));

        }

        public IEnumerable<ClubMembersDTO> GetClubMembersDTO(ClubDTO club, bool isApproved)
        {
            var tempClub = _repoWrapper.Club
                .FindByCondition(i => i.ID == club.ID)
                .FirstOrDefault();
            return _mapper.Map<IEnumerable<DataAccess.Entities.ClubMembers>, IEnumerable<ClubMembersDTO>>(GetClubMembers(tempClub, isApproved));
        }

        public void ToggleIsApprovedInClubMembers(int memberId, int clubId)
        {
            ClubService clubService = new ClubService(_repoWrapper, _mapper, _env);
            var club = clubService.GetByIdWithDetails(clubId);
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
    }
}
