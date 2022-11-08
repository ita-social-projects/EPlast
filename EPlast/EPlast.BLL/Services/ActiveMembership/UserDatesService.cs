using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;

namespace EPlast.BLL.Services.ActiveMembership
{
    public class UserDatesService : IUserDatesService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IUserManagerService _userManagerService;

        public UserDatesService(IMapper mapper, IRepositoryWrapper repoWrapper, IUserManagerService userManagerService)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _userManagerService = userManagerService;
        }

        public async Task<bool> ChangeUserEntryAndOathDateAsync(EntryAndOathDatesDto entryAndOathDatesDTO)
        {
            bool isChanged = false;
            var userDto = await _userManagerService.FindByIdAsync(entryAndOathDatesDTO.UserId);
            if (userDto != null)
            {
                UserMembershipDates userMembershipDates = await _repoWrapper.UserMembershipDates.GetFirstOrDefaultAsync(umd => umd.UserId == userDto.Id);
                if (userMembershipDates != null && entryAndOathDatesDTO.DateEntry != default)
                {
                    var dateOathIsLowerDateEnd = userMembershipDates.DateEnd == default || userMembershipDates.DateEnd > entryAndOathDatesDTO.DateOath;
                    var dateEntryIsLowerOathDate = entryAndOathDatesDTO.DateOath == default || entryAndOathDatesDTO.DateOath > entryAndOathDatesDTO.DateEntry;
                    if (dateEntryIsLowerOathDate && dateOathIsLowerDateEnd)
                    {
                        userMembershipDates.DateOath = entryAndOathDatesDTO.DateOath;
                        userMembershipDates.DateEntry = entryAndOathDatesDTO.DateEntry;
                        _repoWrapper.UserMembershipDates.Update(userMembershipDates);
                        await _repoWrapper.SaveAsync();
                        isChanged = true;
                    }
                }
            }
            return isChanged;
        }

        public async Task<bool> UserHasMembership(string userId)
        {
            try
            {
                await GetUserMembershipDatesAsync(userId);
                return true;
            }
            catch (Exception)
            {
                // ignored
            }

            return false;
        }

        public async Task<UserMembershipDatesDto> GetUserMembershipDatesAsync(string userId)
        {
            var userDto = await _userManagerService.FindByIdAsync(userId);

            if (userDto != null)
            {
                UserMembershipDates userMembershipDates = await _repoWrapper.UserMembershipDates.GetFirstOrDefaultAsync(umd => umd.UserId == userId);

                if (userMembershipDates != null)
                {
                    return _mapper.Map<UserMembershipDatesDto>(userMembershipDates);
                }
            }
            throw new InvalidOperationException();
        }

        public Tuple<IEnumerable<UserFormerMembershipTable>, int> GetUserFormerMembershipDatesTable(string userId)
        {
            var tuple = _repoWrapper.UserFormerMembershipDates.GetUserTableObjects(userId);

            var dates = tuple.Item1;
            var rowCount = tuple.Item2;

            return new Tuple<IEnumerable<UserFormerMembershipTable>, int>(dates, rowCount);
        }

        public async Task<bool> AddDateEntryAsync(string userId)
        {
            var userDto = await _userManagerService.FindByIdAsync(userId);
            if (userDto != null)
            {
                UserMembershipDates userMembershipDates = new UserMembershipDates()
                {
                    UserId = userId,
                    DateEntry = default,
                    DateOath = default,
                    DateEnd = default
                };

                await _repoWrapper.UserMembershipDates.CreateAsync(userMembershipDates);
                await _repoWrapper.SaveAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> AddFormerEntryDateAsync(string userId)
        {
            var userDto = await _userManagerService.FindByIdAsync(userId);
            if (userDto != null)
            {
                UserFormerMembershipDates userFormerMembershipDates = new UserFormerMembershipDates()
                {
                    UserId = userId,
                    DateEntry = DateTime.Now,
                    DateEnd = default
                };

                await _repoWrapper.UserFormerMembershipDates.CreateAsync(userFormerMembershipDates);
                await _repoWrapper.SaveAsync();

                return true;
            }
            return false;
        }


        public async Task EndUserMembership(string userId)
        {
            var membershipDates = await _repoWrapper.UserMembershipDates.GetFirstOrDefaultAsync(m => m.UserId == userId);
            if (membershipDates != null)
            {
                membershipDates.DateEnd = DateTime.Now;
                _repoWrapper.UserMembershipDates.Update(membershipDates);
                await _repoWrapper.SaveAsync();
            }
        }
    }
}
