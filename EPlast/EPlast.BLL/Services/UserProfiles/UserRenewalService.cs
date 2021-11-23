using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Services.UserProfiles
{
    public class UserRenewalService : IUserRenewalService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly ICityService _cityService;
        private readonly ICityParticipantsService _cityParticipantsService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly IEmailContentService _emailContentService;
        private readonly UserManager<User> _userManager;

        public UserRenewalService(
            IRepositoryWrapper repoWrapper,
            IMapper mapper,
            ICityService cityService,
            ICityParticipantsService cityParticipantsService,
            IEmailSendingService emailSendingService,
            IEmailContentService emailContentService,
            UserManager<User> userManager)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _cityService = cityService;
            _cityParticipantsService = cityParticipantsService;
            _emailSendingService = emailSendingService;
            _emailContentService = emailContentService;
            _userManager = userManager;
        }
        
        /// <inheritdoc />
        public async Task AddUserRenewalAsync(UserRenewalDTO userRenewal)
        {
            var renewal = _mapper.Map<UserRenewalDTO, UserRenewal>(userRenewal);
            await _repoWrapper.UserRenewal.CreateAsync(renewal);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task ChangeUserRenewalAsync(UserRenewalDTO userRenewal)
        {
            var renewal = await _repoWrapper.UserRenewal.GetFirstAsync(x => x.Id == userRenewal.Id);
            renewal.Approved = userRenewal.Approved;
            _repoWrapper.UserRenewal.Update(renewal);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<bool> IsValidUserRenewalAsync(UserRenewalDTO userRenewal)
        {
            var user = await _userManager.FindByIdAsync(userRenewal.UserId);
            if (user == null || !await _userManager.IsInRoleAsync(user, Roles.FormerPlastMember))
                return false;
            
            var renewals =
                await _repoWrapper.UserRenewal.GetAllAsync(renewal => renewal.UserId == userRenewal.UserId);

            if (!renewals.Any()) return true;
            return (DateTime.Now - renewals.Last().RequestDate).Days >= 10;
        }

        /// <inheritdoc />
        public async Task<bool> IsValidAdminAsync(User user, int cityId)
        {
            if ((await _userManager.GetRolesAsync(user)).Contains(Roles.Admin)) 
                return true;

            var validAdmins = (await _cityService.GetCityAdminsIdsAsync(cityId)).Split(",");
            return validAdmins.Any(admin => user.Id == admin);
        }

        /// <inheritdoc />
        public IEnumerable<UserRenewalsTableObject> GetUserRenewalsTableObject(string searchedData, int page,
            int pageSize)
        {
            return _repoWrapper.UserRenewal.GetUserRenewals(searchedData, page, pageSize);
        }
        
        /// <inheritdoc />
        public async Task SendRenewalConfirmationEmailAsync(string userId, int cityId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var cityName = (await _cityService.GetCityByIdAsync(cityId)).Name;
            var emailContent = _emailContentService.GetUserRenewalConfirmationEmail(cityName);
            await _emailSendingService.SendEmailAsync(user.Email, emailContent.Subject, emailContent.Message,
                emailContent.Title);
        }

        /// <inheritdoc />
        public async Task<CityMembersDTO> RenewFormerMemberUserAsync(UserRenewalDTO userRenewal)
        {
            var userId = userRenewal.UserId;
            var cityId = userRenewal.CityId;
            var user = await _userManager.FindByIdAsync(userId);

            if (!await _userManager.IsInRoleAsync(user, Roles.FormerPlastMember)) 
                throw new ArgumentException("User is not Former-Member", nameof(userRenewal));

            await _userManager.RemoveFromRoleAsync(user, Roles.FormerPlastMember);
            await _userManager.AddToRoleAsync(user, Roles.RegisteredUser);
            await ResolveUserMembershipDatesAsync(userId);
            var newUser = await _cityParticipantsService.AddFollowerAsync(cityId, userId);
            await ChangeUserRenewalAsync(userRenewal);

            return newUser;
        }

        /// <inheritdoc />
        public async Task ResolveUserMembershipDatesAsync(string userId)
        {
            var membershipDates = await _repoWrapper.UserMembershipDates.GetFirstAsync(date => date.UserId == userId);
            membershipDates.DateEntry = DateTime.MinValue;
            membershipDates.DateEnd = DateTime.MinValue;
            membershipDates.DateOath = DateTime.MinValue;
            _repoWrapper.UserMembershipDates.Update(membershipDates);
            await _repoWrapper.SaveAsync();
        }
    }
}
