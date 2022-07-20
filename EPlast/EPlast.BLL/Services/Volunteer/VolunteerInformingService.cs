using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Notification;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Interfaces.Volunteer;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.DataAccess.Repositories.Realizations.Base;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services.Volunteer
{
    public class VolunteerInformingService : IVolunteerInformingService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly INotificationService _notificationService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly IEmailContentService _emailContentService;

        public VolunteerInformingService(IRepositoryWrapper repositoryWrapper,
            INotificationService notificationService, IUserService userService, UserManager<User> userManager,
            IEmailSendingService emailSendingService, IEmailContentService emailContentService)
        {
            _repositoryWrapper = repositoryWrapper;
            _notificationService = notificationService;
            _userService = userService;
            _userManager = userManager;
            _emailSendingService = emailSendingService;
            _emailContentService = emailContentService;
        }

        public async Task SendNewVolunteerNotificationToAdministratorsAsync(string volunteerId)
        {
            var volunteer = _repositoryWrapper.User
                .Include(u => u.CityMembers)
                .FirstOrDefault(u => u.Id == volunteerId);

            if (volunteer != null)
            {
                var cityId = volunteer.CityMembers.SingleOrDefault()?.CityId;

                if (cityId != null)
                {
                    await SendNotificationToCityAdministration((int)cityId, volunteer);
                }
                else
                {
                    await SendNotificationToRegionAdministration(volunteer.RegionId, volunteer);
                }
            }
        }

        public async Task SendNewVolunteerEmailToAdministratorsAsync(string volunteerId)
        {
            var volunteer = _repositoryWrapper.User
                .Include(u => u.CityMembers)
                .FirstOrDefault(u => u.Id == volunteerId);

            if (volunteer != null)
            {
                var cityId = volunteer.CityMembers.SingleOrDefault()?.CityId;
                if (cityId != null)
                {
                    await SendEmailToCityAdministration((int)cityId, volunteer);
                }
                else
                {
                    await SendEmailToRegionAdministration(volunteer.RegionId, volunteer);
                }
            }

        }

        private async Task SendEmailToRegionAdministration(int regionId, User volunteer)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration
                .GetAllAsync(i => i.CityId == regionId &&
                                  i.AdminType.AdminTypeName == Roles.CityHead
                                  || i.AdminType.AdminTypeName == Roles.CityHeadDeputy
                                  || i.AdminType.AdminTypeName == Roles.CityReferentUPS
                                  || i.AdminType.AdminTypeName == Roles.CityReferentUSP
                                  || i.AdminType.AdminTypeName == Roles.CityReferentOfActiveMembership,
                    i => i
                        .Include(c => c.AdminType)
                        .Include(a => a.User));

            if (cityAdministration != null)
            {
                var emailContent = await _emailContentService.GetCityAdminAboutNewFollowerEmailAsync(volunteer.Id,
                    volunteer.FirstName, volunteer.LastName, false);

                foreach (var admin in cityAdministration)
                {
                    await _emailSendingService.SendEmailAsync(admin.User.Email, emailContent.Subject,
                        emailContent.Message,
                        emailContent.Title);
                }
            }
        }

        private async Task SendEmailToCityAdministration(int cityId, User volunteer)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration
                .GetAllAsync(i => i.CityId == cityId &&
                                  i.AdminType.AdminTypeName == Roles.CityHead
                                  || i.AdminType.AdminTypeName == Roles.CityHeadDeputy
                                  || i.AdminType.AdminTypeName == Roles.CityReferentUPS
                                  || i.AdminType.AdminTypeName == Roles.CityReferentUSP
                                  || i.AdminType.AdminTypeName == Roles.CityReferentOfActiveMembership,
                    i => i
                        .Include(c => c.AdminType)
                        .Include(a => a.User));

            if (cityAdministration != null)
            {
                var emailContent = await _emailContentService.GetCityAdminAboutNewFollowerEmailAsync(volunteer.Id,
                    volunteer.FirstName, volunteer.LastName, false);

                foreach (var admin in cityAdministration)
                {
                    await _emailSendingService.SendEmailAsync(admin.User.Email, emailContent.Subject,
                        emailContent.Message,
                        emailContent.Title);
                }
            }
        }

        private async Task SendNotificationToCityAdministration(int cityId, User volunteer)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration
                .GetAllAsync(i => i.CityId == cityId &&
                                  i.AdminType.AdminTypeName == Roles.CityHead
                                  || i.AdminType.AdminTypeName == Roles.CityHeadDeputy
                                  || i.AdminType.AdminTypeName == Roles.CityReferentUPS
                                  || i.AdminType.AdminTypeName == Roles.CityReferentUSP
                                  || i.AdminType.AdminTypeName == Roles.CityReferentOfActiveMembership,
                    i => i
                        .Include(c => c.AdminType)
                        .Include(a => a.User));

            if (cityAdministration != null)
            {

                List<UserNotificationDto> userNotificationsDto = new List<UserNotificationDto>();

                foreach (var admin in cityAdministration)
                {
                    userNotificationsDto.Add(new UserNotificationDto
                    {
                        Message =
                            $"До твоєї станиці хоче доєднатися волонтер {volunteer.FirstName} {volunteer.LastName}",
                        NotificationTypeId = 1,
                        OwnerUserId = admin.UserId,
                        SenderLink = $"/user/table?search={volunteer.FirstName} {volunteer.LastName}",
                        SenderName = "Переглянути"
                    });
                }

                await _notificationService.AddListUserNotificationAsync(userNotificationsDto);
            }

        }

        private async Task SendNotificationToRegionAdministration(int regionId, User volunteer)
        {
            var regionAdministration = await _repositoryWrapper.RegionAdministration
                .GetAllAsync(i => i.RegionId == regionId &&
                                  i.AdminType.AdminTypeName == Roles.OkrugaHead
                                  || i.AdminType.AdminTypeName == Roles.OkrugaHeadDeputy
                                  || i.AdminType.AdminTypeName == Roles.OkrugaReferentUPS
                                  || i.AdminType.AdminTypeName == Roles.OkrugaReferentUSP
                                  || i.AdminType.AdminTypeName == Roles.OkrugaReferentOfActiveMembership,
                    i => i
                        .Include(c => c.AdminType)
                        .Include(a => a.User));

            if (regionAdministration != null)
            {
                List<UserNotificationDto> userNotificationsDto = new List<UserNotificationDto>();

                foreach (var admin in regionAdministration)
                {
                    userNotificationsDto.Add(new UserNotificationDto
                    {
                        Message =
                            $"До твоєї округи хоче доєднатися волонтер {volunteer.FirstName} {volunteer.LastName}",
                        NotificationTypeId = 1,
                        OwnerUserId = admin.UserId,
                        SenderLink = $"/user/table?search={volunteer.FirstName} {volunteer.LastName}",
                        SenderName = "Переглянути"
                    });
                }

                await _notificationService.AddListUserNotificationAsync(userNotificationsDto);
            }

        }

    }
}
