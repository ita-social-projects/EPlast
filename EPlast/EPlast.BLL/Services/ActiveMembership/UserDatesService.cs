using AutoMapper;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System;
using System.Threading.Tasks;

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

        public async Task<bool> ChangeUserEntryAndOathDateAsync(EntryAndOathDatesDTO entryAndOathDatesDTO)
        {
            bool isChanged = false;
            var userDto = await _userManagerService.FindByIdAsync(entryAndOathDatesDTO.UserId);
            if (userDto != null)
            {
                UserMembershipDates userMembershipDates = await _repoWrapper.UserMembershipDates.GetFirstOrDefaultAsync(umd => umd.UserId == userDto.Id);
                if (userMembershipDates != null)
                {
                    var dateOathIsLowerDateEnd = userMembershipDates.DateEnd == default || userMembershipDates.DateEnd > entryAndOathDatesDTO.DateOath;
                    if (userMembershipDates.DateEntry <= entryAndOathDatesDTO.DateOath && dateOathIsLowerDateEnd)
                    {
                        userMembershipDates.DateOath = entryAndOathDatesDTO.DateOath;
                        userMembershipDates.DateEntry = entryAndOathDatesDTO.EntryDate;
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

        public async Task<UserMembershipDatesDTO> GetUserMembershipDatesAsync(string userId)
        {
            var userDto = await _userManagerService.FindByIdAsync(userId);

            if (userDto != null)
            {
                UserMembershipDates userMembershipDates = await _repoWrapper.UserMembershipDates.GetFirstOrDefaultAsync(umd => umd.UserId == userId);

                if (userMembershipDates != null)
                {
                    return _mapper.Map<UserMembershipDatesDTO>(userMembershipDates);
                }
            }
            throw new InvalidOperationException();
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
