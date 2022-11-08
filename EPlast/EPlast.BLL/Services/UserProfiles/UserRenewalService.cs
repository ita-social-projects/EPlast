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
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Services.UserProfiles
{
    public class UserRenewalService : IUserRenewalService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ICityParticipantsService _cityParticipantsService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly IEmailContentService _emailContentService;
        private readonly UserManager<User> _userManager;

        public UserRenewalService(
            IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IMediator mediator,
            ICityParticipantsService cityParticipantsService,
            IEmailSendingService emailSendingService,
            IEmailContentService emailContentService,
            UserManager<User> userManager)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _mediator = mediator;
            _cityParticipantsService = cityParticipantsService;
            _emailSendingService = emailSendingService;
            _emailContentService = emailContentService;
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task AddUserRenewalAsync(UserRenewalDto userRenewal)
        {
            var renewal = _mapper.Map<UserRenewalDto, UserRenewal>(userRenewal);
            await _repoWrapper.UserRenewal.CreateAsync(renewal);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task ChangeUserRenewalAsync(UserRenewalDto userRenewal)
        {
            var renewal = await _repoWrapper.UserRenewal.GetFirstAsync(x => x.Id == userRenewal.Id);
            renewal.Approved = userRenewal.Approved;
            _repoWrapper.UserRenewal.Update(renewal);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<bool> IsValidUserRenewalAsync(UserRenewalDto userRenewal)
        {
            var user = await _userManager.FindByIdAsync(userRenewal.UserId);
            if (user == null || !await _userManager.IsInRoleAsync(user, Roles.FormerPlastMember))
                return false;

            const int requestDaysLimit = 10;
            var renewals =
                await _repoWrapper.UserRenewal.GetAllAsync(renewal => renewal.UserId == userRenewal.UserId);

            if (!renewals.Any()) return true;
            return (DateTime.Now - renewals.Last().RequestDate).Days >= requestDaysLimit;
        }

        /// <inheritdoc />
        public async Task<bool> IsValidAdminAsync(User user, int cityId)
        {
            if ((await _userManager.GetRolesAsync(user)).Contains(Roles.Admin))
                return true;
            var query = new GetCityAdminsIdsQuery(cityId);
            var validAdmins = (await _mediator.Send(query)).Split(",");
            return validAdmins.Any(admin => user.Id == admin);
        }

        /// <inheritdoc />
        public IEnumerable<UserRenewalsTableObject> GetUserRenewalsTableObject(string searchedData, int page,
            int pageSize, string filter)
        {
            return _repoWrapper.UserRenewal.GetUserRenewals(searchedData, page, pageSize, filter);
        }

        /// <inheritdoc />
        public async Task SendRenewalConfirmationEmailAsync(string userId, int cityId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var query = new GetCityByIdQuery(cityId);
            var cityName = (await _mediator.Send(query)).Name;
            var emailContent = _emailContentService.GetUserRenewalConfirmationEmail(cityName);
            await _emailSendingService.SendEmailAsync(user.Email, emailContent.Subject, emailContent.Message,
                emailContent.Title);
        }

        /// <inheritdoc />
        public async Task<CityMembersDto> RenewFormerMemberUserAsync(UserRenewalDto userRenewal)
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

            var formerMembershipDates = await _repoWrapper.UserFormerMembershipDates.GetFirstOrDefaultAsync(m => m.UserId == userId);
            if (formerMembershipDates != null)
            {
                formerMembershipDates.DateEnd = DateTime.Now;
                _repoWrapper.UserFormerMembershipDates.Update(formerMembershipDates);
                await _repoWrapper.SaveAsync();
            }

            await ChangeUserRenewalAsync(userRenewal);

            return newUser;
        }

        /// <inheritdoc />
        public async Task ResolveUserMembershipDatesAsync(string userId)
        {
            var membershipDates = await _repoWrapper.UserMembershipDates.GetFirstAsync(date => date.UserId == userId);
            membershipDates.DateEntry = DateTime.MinValue;
            membershipDates.DateEnd = DateTime.MinValue;
            _repoWrapper.UserMembershipDates.Update(membershipDates);
            await _repoWrapper.SaveAsync();
        }
    }
}
