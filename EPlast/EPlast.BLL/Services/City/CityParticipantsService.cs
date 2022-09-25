using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Notification;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.HostURL;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services.City
{
    public class CityParticipantsService : ICityParticipantsService
    {
        private readonly IAdminTypeService _adminTypeService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly IEmailContentService _emailContentService;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        private readonly INotificationService _notificationService;
        private readonly IMediator _mediator;
        private readonly IHostURLService _hostURLService;

        public CityParticipantsService(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            UserManager<User> userManager,
            IAdminTypeService adminTypeService,
            IEmailSendingService emailSendingService,
            IEmailContentService emailContentService,
            IMediator mediator,
            INotificationService notificationService,
            IHostURLService hostURLService
        )
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
            _adminTypeService = adminTypeService;
            _emailSendingService = emailSendingService;
            _emailContentService = emailContentService;
            _mediator = mediator;
            _notificationService = notificationService;
            _hostURLService = hostURLService;
        }

        /// <inheritdoc />
        public async Task<CityAdministrationDto> AddAdministratorAsync(CityAdministrationDto adminDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);
            var headType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.CityHead);
            var headDeputyType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.CityHeadDeputy);
            var newAdmin = new CityAdministration()
            {
                StartDate = adminDTO.StartDate ?? DateTime.Now,
                EndDate = adminDTO.EndDate,
                AdminTypeId = adminType.ID,
                CityId = adminDTO.CityId,
                UserId = adminDTO.User.ID,
                Status = adminDTO.Status
            };

            if (CheckCityWasAdmin(newAdmin))
            {
                newAdmin.Status = false;
                await _repositoryWrapper.CityAdministration.CreateAsync(newAdmin);
                await _repositoryWrapper.SaveAsync();
                adminDTO.ID = newAdmin.ID;
                adminDTO.UserId = newAdmin.UserId;
                return adminDTO;
            }

            var user = await _userManager.FindByIdAsync(adminDTO.User.ID);
            string role = adminType.AdminTypeName switch
            {
                Roles.CityHead => Roles.CityHead,
                Roles.CityHeadDeputy => Roles.CityHeadDeputy,
                Roles.CityReferentUPS => Roles.CityReferentUPS,
                Roles.CityReferentUSP => Roles.CityReferentUSP,
                Roles.CityReferentOfActiveMembership => Roles.CityReferentOfActiveMembership,
                _ => Roles.CitySecretary,
            };
            await _userManager.AddToRoleAsync(user, role);
            if (adminType.AdminTypeName == headType.AdminTypeName)
            {
                var headDeputy = await _repositoryWrapper.CityAdministration
                    .GetFirstOrDefaultAsync(a => a.AdminTypeId == headDeputyType.ID && a.CityId == adminDTO.CityId && a.Status);
                if (headDeputy != null && headDeputy.UserId == adminDTO.UserId)
                {
                    await RemoveAdministratorAsync(headDeputy.ID);
                }
            }

            await CheckCityHasAdminAsync(adminDTO.CityId, adminType.AdminTypeName, newAdmin);

            await _repositoryWrapper.CityAdministration.CreateAsync(newAdmin);
            await _repositoryWrapper.SaveAsync();
            adminDTO.ID = newAdmin.ID;
            adminDTO.UserId = newAdmin.UserId;
            return adminDTO;
        }

        /// <inheritdoc />
        public async Task<CityMembersDto> AddFollowerAsync(int cityId, string userId)
        {
            var oldCityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(i => i.UserId == userId);
            if (oldCityMember != null)
            {
                _repositoryWrapper.CityMembers.Delete(oldCityMember);
                await _repositoryWrapper.SaveAsync();
            }

            var oldCityAdmins = await _repositoryWrapper.CityAdministration
                .GetAllAsync(i => i.UserId == userId && (DateTime.Now < i.EndDate || i.EndDate == null));
            foreach (var admin in oldCityAdmins)
            {
                await RemoveAdministratorAsync(admin.ID);
            }

            var cityMember = new CityMembers()
            {
                CityId = cityId,
                IsApproved = false,
                UserId = userId,
                User = await _userManager.FindByIdAsync(userId)
            };
            await _repositoryWrapper.CityMembers.CreateAsync(cityMember);

            var cityDTO = await _mediator.Send(
                new GetCityByIdQuery(cityId)
            );

            var regionAdministrations =
                await _repositoryWrapper.RegionAdministration.GetAllAsync(d =>
                    d.UserId == userId && d.Status && d.RegionId != cityDTO.RegionId);
            if (regionAdministrations != null)
            {
                foreach (var elem in regionAdministrations)
                {
                    elem.EndDate = DateTime.Now;
                    elem.Status = false;
                    _repositoryWrapper.RegionAdministration.Update(elem);
                }
            }

            await _repositoryWrapper.SaveAsync();

            await SendEmailCityAdminAboutNewFollowerAsync(cityMember.CityId, cityMember.User);
            await SendNotificationCityAdminAboutNewFollowerAsync(cityId, cityMember.User);

            return _mapper.Map<CityMembers, CityMembersDto>(cityMember);
        }

        /// <inheritdoc />
        public async Task<CityMembersDto> AddFollowerAsync(int cityId, User user)
        {
            await _userManager.RemoveFromRolesAsync(user, Roles.DeleteableListOfRoles);
            return await AddFollowerAsync(cityId, await _userManager.GetUserIdAsync(user));
        }

        public async Task AddNotificationUserWithoutSelectedCity(User user, int regionId)
        {
            string message, senderLink;
            if (await _userManager.IsInRoleAsync(user, Roles.RegisteredUser))
            {
                message = $"До Твоєї станиці хоче доєднатися волонтер {user.FirstName} {user.LastName}";
                senderLink = $"/user/table?search={user.FirstName} {user.LastName}&tab=registered";
            }
            else
            {
                message = $"До Твоєї станиці хоче доєднатися користувач {user.FirstName} {user.LastName}";
                senderLink = $"/user/table?search={user.FirstName} {user.LastName}";
            }
            List<UserNotificationDto> userNotificationsDTO = new List<UserNotificationDto>();

            var regionAdministration = await _repositoryWrapper.RegionAdministration
                .GetAllAsync(i => i.RegionId == regionId,
                    i => i
                        .Include(c => c.AdminType)
                        .Include(a => a.User));

            var regionHead = regionAdministration.FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.OkrugaHead
                                                                      && (DateTime.Now < a.EndDate ||
                                                                          a.EndDate == null));
            var regionHeadDeputy = regionAdministration.FirstOrDefault(a =>
                a.AdminType.AdminTypeName == Roles.OkrugaHeadDeputy
                && (DateTime.Now < a.EndDate || a.EndDate == null));

            var regionReferentsUPS =
                regionAdministration.Where(a => a.AdminType.AdminTypeName == Roles.OkrugaReferentUPS
                                                && (DateTime.Now < a.EndDate || a.EndDate == null)).ToList();
            var regionReferentsUSP =
                regionAdministration.Where(a => a.AdminType.AdminTypeName == Roles.OkrugaReferentUSP
                                                && (DateTime.Now < a.EndDate || a.EndDate == null)).ToList();
            var regionReferentsOfActiveMembership = regionAdministration.Where(a =>
                a.AdminType.AdminTypeName == Roles.OkrugaReferentOfActiveMembership
                && (DateTime.Now < a.EndDate || a.EndDate == null)).ToList();

            var emailContent = await _emailContentService.GetRegionAdminAboutNewFollowerEmailAsync(user.Id,
                user.FirstName, user.LastName, false);

            if (regionHead != null)
            {
                userNotificationsDTO.Add(new UserNotificationDto
                {
                    Message = message,
                    NotificationTypeId = 1,
                    OwnerUserId = regionHead.UserId,
                    SenderLink = senderLink,
                    SenderName = "Переглянути"
                });

                await _emailSendingService.SendEmailAsync(regionHead.User.Email, emailContent.Subject,
                    emailContent.Message,
                    emailContent.Title);
            }

            if (regionHeadDeputy != null)
            {
                userNotificationsDTO.Add(new UserNotificationDto
                {
                    Message = message,
                    NotificationTypeId = 1,
                    OwnerUserId = regionHeadDeputy.UserId,
                    SenderLink = senderLink,
                    SenderName = "Переглянути"
                });

                await _emailSendingService.SendEmailAsync(regionHeadDeputy.User.Email, emailContent.Subject,
                    emailContent.Message,
                    emailContent.Title);
            }

            if (!regionReferentsUPS.Any())
            {
                foreach (var referent in regionReferentsUPS)
                {
                    userNotificationsDTO.Add(new UserNotificationDto
                    {
                        Message = message,
                        NotificationTypeId = 1,
                        OwnerUserId = referent.UserId,
                        SenderLink = senderLink,
                        SenderName = "Переглянути"
                    });

                    await _emailSendingService.SendEmailAsync(referent.User.Email, emailContent.Subject,
                        emailContent.Message,
                        emailContent.Title);
                }
            }

            if (regionReferentsUSP.Count != 0)
            {
                foreach (var referent in regionReferentsUSP)
                {
                    userNotificationsDTO.Add(new UserNotificationDto
                    {
                        Message = message,
                        NotificationTypeId = 1,
                        OwnerUserId = referent.UserId,
                        SenderLink = senderLink,
                        SenderName = "Переглянути"
                    });
                    await _emailSendingService.SendEmailAsync(referent.User.Email, emailContent.Subject,
                        emailContent.Message,
                        emailContent.Title);
                }
            }

            if (regionReferentsOfActiveMembership.Count != 0)
            {
                foreach (var referent in regionReferentsOfActiveMembership)
                {
                    userNotificationsDTO.Add(new UserNotificationDto
                    {
                        Message = message,
                        NotificationTypeId = 1,
                        OwnerUserId = referent.UserId,
                        SenderLink = senderLink,
                        SenderName = "Переглянути"
                    });
                    await _emailSendingService.SendEmailAsync(referent.User.Email, emailContent.Subject,
                        emailContent.Message,
                        emailContent.Title);
                }
            }

            await _notificationService.AddListUserNotificationAsync(userNotificationsDTO);
        }

        public async Task ContinueAdminsDueToDate()
        {
            var admins = await _repositoryWrapper.CityAdministration.GetAllAsync(x => x.Status);

            foreach (var admin in admins)
            {
                if (admin.EndDate != null && DateTime.Compare((DateTime)admin.EndDate, DateTime.Now) < 0)
                {
                    admin.EndDate = admin.EndDate.Value.AddYears(1);
                    _repositoryWrapper.CityAdministration.Update(admin);
                }
            }
            await _repositoryWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<CityAdministrationDto> EditAdministratorAsync(CityAdministrationDto adminDTO)
        {
            var admin = await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(a => a.ID == adminDTO.ID);
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);

            if (adminType.ID == admin.AdminTypeId)
            {
                admin.StartDate = adminDTO.StartDate ?? DateTime.Now;
                admin.EndDate = adminDTO.EndDate;
                admin.Status = true;

                _repositoryWrapper.CityAdministration.Update(admin);
                await _repositoryWrapper.SaveAsync();
                return adminDTO;
            }

            await RemoveAdministratorAsync(adminDTO.ID);
            adminDTO = await AddAdministratorAsync(adminDTO);
            return adminDTO;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CityAdministrationDto>> GetAdministrationByIdAsync(int cityId)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration.GetAllAsync(
                predicate: x => x.CityId == cityId,
                include: x => x.Include(q => q.User).
                     Include(q => q.AdminType));

            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDto>>(cityAdministration);
        }

        public async Task<IEnumerable<CityAdministrationDto>> GetAdministrationsOfUserAsync(string UserId)
        {
            var admins = await _repositoryWrapper.CityAdministration.GetAllAsync(a => a.UserId == UserId && a.Status,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType).Include(a => a.City)
                 );

            foreach (var admin in admins.Where(x => x.City != null))
            {
                admin.City.CityAdministration = null;
            }

            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDto>>(admins);
        }

        public async Task<IEnumerable<CityAdministrationStatusDto>> GetAdministrationStatuses(string UserId)
        {
            var cityAdmins = await _repositoryWrapper.CityAdministration.GetAllAsync(a => a.UserId == UserId && !a.Status,
                             include:
                             source => source.Include(c => c.User).Include(c => c.AdminType).Include(c => c.City)
                             );
            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationStatusDto>>(cityAdmins);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CityMembersDto>> GetMembersByCityIdAsync(int cityId)
        {
            var cityMembers = await _repositoryWrapper.CityMembers.GetAllAsync(
                    predicate: c => c.CityId == cityId && c.EndDate == null,
                    include: source => source
                        .Include(c => c.User));

            return _mapper.Map<IEnumerable<CityMembers>, IEnumerable<CityMembersDto>>(cityMembers);
        }

        public async Task<IEnumerable<CityAdministrationDto>> GetPreviousAdministrationsOfUserAsync(string UserId)
        {
            var admins = await _repositoryWrapper.CityAdministration.GetAllAsync(a => a.UserId == UserId && !a.Status,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType).Include(a => a.City)
                 );

            foreach (var admin in admins)
            {
                admin.City.CityAdministration = null;
            }

            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDto>>(admins).Reverse();
        }

        /// <inheritdoc />
        public async Task RemoveAdministratorAsync(int adminId)
        {
            var admin = await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(u => u.ID == adminId);
            admin.EndDate = DateTime.Now;
            admin.Status = false;

            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(admin.AdminTypeId);
            var user = await _userManager.FindByIdAsync(admin.UserId);
            string role = adminType.AdminTypeName switch
            {
                Roles.CityHead => Roles.CityHead,
                Roles.CityHeadDeputy => Roles.CityHeadDeputy,
                Roles.CityReferentUPS => Roles.CityReferentUPS,
                Roles.CityReferentUSP => Roles.CityReferentUSP,
                Roles.CityReferentOfActiveMembership => Roles.CityReferentOfActiveMembership,
                _ => Roles.CitySecretary,
            };

            if (role != Roles.CitySecretary || (await CheckUserHasOneSecretaryTypeForCityAsync(admin)))
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            _repositoryWrapper.CityAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task RemoveFollowerAsync(int followerId, string comment)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.ID == followerId,
                    m => m.Include(u => u.User)
                        .Include(u => u.City));

            _repositoryWrapper.CityMembers.Delete(cityMember);
            await _repositoryWrapper.SaveAsync();

            await SendEmailRemoveCityFollowerAsync(cityMember.User.Email, cityMember.City, comment);
        }

        public async Task RemoveMemberAsync(string userId)
        {
            var cityMember = await _repositoryWrapper.CityMembers.GetFirstOrDefaultAsync(m => m.UserId == userId);

            if (cityMember != null)
            {
                _repositoryWrapper.CityMembers.Delete(cityMember);
                await _repositoryWrapper.SaveAsync();
            }
        }

        /// <inheritdoc />
        public async Task<CityMembersDto> ToggleApproveStatusAsync(int memberId)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.ID == memberId,
                                        m => m.Include(u => u.User)
                                              .Include(u => u.City));
            cityMember.IsApproved = !cityMember.IsApproved;
            _repositoryWrapper.CityMembers.Update(cityMember);
            await _repositoryWrapper.SaveAsync();
            await ChangeMembershipDatesByApprove(cityMember.UserId, cityMember.IsApproved);
            var cityMemberDto = _mapper.Map<CityMembers, CityMembersDto>(cityMember);
            var user = await _userManager.FindByIdAsync(cityMember.UserId);
            if (cityMember.IsApproved && await _userManager.IsInRoleAsync(user, Roles.RegisteredUser))
            {
                await GiveUserSupporterRole(user);
                cityMemberDto.WasInRegisteredUserRole = true;
            }
            await SendEmailCityApproveStatusAsync(cityMember.User.Email, cityMember.UserId, cityMember.City, cityMember.IsApproved);
            return cityMemberDto;
        }

        public async Task<bool?> CheckIsUserApproved(int userId)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.ID == userId);
            return cityMember?.IsApproved;
        }

        public async Task<string> CityOfApprovedMember(string memberId)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.UserId == memberId,
                                        m => m.Include(u => u.City));

            if (cityMember == null)
            {
                return null;
            }
            if (cityMember.IsApproved)
            {
                return cityMember.City.Name;
            }
            return cityMember.City.Name = null;
        }

        private async Task ChangeMembershipDatesByApprove(string userId, bool isApproved)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (!await _userManager.IsInRoleAsync(user, Roles.PlastMember) && user != null)
            {
                var userMembershipDates = await _repositoryWrapper.UserMembershipDates
                            .GetFirstOrDefaultAsync(umd => umd.UserId == userId);
                //Change entry date and register date only If the user enters the city for the first time
                if (userMembershipDates.DateEntry == default)
                {
                    userMembershipDates.DateEntry = isApproved ? DateTime.Now : default;
                    user.RegistredOn = DateTime.Now;
                    _repositoryWrapper.UserMembershipDates.Update(userMembershipDates);
                    await _repositoryWrapper.SaveAsync();
                }
            }
        }

        private async Task CheckCityHasAdminAsync(int cityId, string adminTypeName, CityAdministration newAdmin)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminTypeName);
            var admin = await _repositoryWrapper.CityAdministration.
                GetFirstOrDefaultAsync(a => a.AdminTypeId == adminType.ID && a.CityId == cityId && a.Status);

            if (!(newAdmin.EndDate == null || DateTime.Today < newAdmin.EndDate))
            {
                newAdmin.Status = false;
                return;
            }
            if (admin != null)
            {
                if (newAdmin.EndDate == null || admin.EndDate < newAdmin.EndDate)
                {
                    await RemoveAdministratorAsync(admin.ID);
                    newAdmin.Status = true;
                }
            }
            else
            {
                newAdmin.Status = true;
            }
        }

        private bool CheckCityWasAdmin(CityAdministration newAdmin)
        {
            return !(newAdmin.EndDate == null || DateTime.Today < newAdmin.EndDate);
        }

        private async Task GiveUserSupporterRole(User user)
        {
            await _userManager.RemoveFromRoleAsync(user, Roles.RegisteredUser);
            await _userManager.AddToRoleAsync(user, Roles.Supporter);
            var emailContent = _emailContentService.GetCityToSupporterRoleOnApproveEmail();
            await _emailSendingService.SendEmailAsync(user.Email, emailContent.Subject, emailContent.Message,
                emailContent.Title);
        }

        private async Task SendEmailCityAdminAboutNewFollowerAsync(int cityId, User user)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration
                .GetAllAsync(i => i.CityId == cityId,
                    i => i
                        .Include(c => c.AdminType)
                        .Include(a => a.User));
            var cityHead = cityAdministration.FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHead
                                                                  && (DateTime.Now < a.EndDate || a.EndDate == null));
            var cityHeadDeputy = cityAdministration.FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHeadDeputy
                                                                  && (DateTime.Now < a.EndDate || a.EndDate == null));
            var cityReferentsUPS =
                cityAdministration.Where(a => a.AdminType.AdminTypeName == Roles.CityReferentUPS
                                              && (DateTime.Now < a.EndDate || a.EndDate == null)).ToList();
            var cityReferentsUSP =
                cityAdministration.Where(a => a.AdminType.AdminTypeName == Roles.CityReferentUSP
                                              && (DateTime.Now < a.EndDate || a.EndDate == null)).ToList();
            var cityReferentsOfActiveMembership = cityAdministration.Where(a =>
                a.AdminType.AdminTypeName == Roles.CityReferentOfActiveMembership
                && (DateTime.Now < a.EndDate || a.EndDate == null)).ToList();

            var emailContent = await _emailContentService.GetCityAdminAboutNewFollowerEmailAsync(user.Id,
                user.FirstName, user.LastName, false, await _userManager.IsInRoleAsync(user, Roles.RegisteredUser));
            if (cityHead != null)
            {
                await _emailSendingService.SendEmailAsync(cityHead.User.Email, emailContent.Subject,
                    emailContent.Message,
                    emailContent.Title);
            }
            if (cityHeadDeputy != null)
            {
                await _emailSendingService.SendEmailAsync(cityHeadDeputy.User.Email, emailContent.Subject,
                    emailContent.Message,
                    emailContent.Title);
            }

            if (cityReferentsUPS.Count != 0)
            {
                foreach (var referent in cityReferentsUPS)
                {
                    await _emailSendingService.SendEmailAsync(referent.User.Email, emailContent.Subject,
                        emailContent.Message,
                        emailContent.Title);
                }
            }
            if (cityReferentsUSP.Count != 0)
            {
                foreach (var referent in cityReferentsUSP)
                {
                    await _emailSendingService.SendEmailAsync(referent.User.Email, emailContent.Subject,
                        emailContent.Message,
                        emailContent.Title);
                }
            }
            if (cityReferentsOfActiveMembership.Count != 0)
            {
                foreach (var referent in cityReferentsOfActiveMembership)
                {
                    await _emailSendingService.SendEmailAsync(referent.User.Email, emailContent.Subject,
                        emailContent.Message,
                        emailContent.Title);
                }
            }
        }

        public async Task SendNotificationCityAdminAboutNewFollowerAsync(int cityId, User user)
        {
            string message, senderLink;
            if (await _userManager.IsInRoleAsync(user, Roles.RegisteredUser))
            {
                message = $"До Твоєї станиці хоче доєднатися волонтер {user.FirstName} {user.LastName}";
                senderLink = $"/user/table?search={user.FirstName} {user.LastName}&tab=registered";
            }
            else
            {
                message = $"До Твоєї станиці хоче доєднатися користувач {user.FirstName} {user.LastName}";
                senderLink = $"/user/table?search={user.FirstName} {user.LastName}";
            }

            var cityAdministration = await _repositoryWrapper.CityAdministration
                .GetAllAsync(i => i.CityId == cityId,
                    i => i
                        .Include(c => c.AdminType)
                        .Include(a => a.User));
            var cityHead = cityAdministration.FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHead
                                                                  && (DateTime.Now < a.EndDate || a.EndDate == null));
            var cityHeadDeputy = cityAdministration.FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHeadDeputy
                                                                  && (DateTime.Now < a.EndDate || a.EndDate == null));

            var cityReferentsUPS =
                cityAdministration.Where(a => a.AdminType.AdminTypeName == Roles.CityReferentUPS
                                                       && (DateTime.Now < a.EndDate || a.EndDate == null)).ToList();
            var cityReferentsUSP =
                cityAdministration.Where(a => a.AdminType.AdminTypeName == Roles.CityReferentUSP
                                              && (DateTime.Now < a.EndDate || a.EndDate == null)).ToList();
            var cityReferentsOfActiveMembership = cityAdministration.Where(a =>
                a.AdminType.AdminTypeName == Roles.CityReferentOfActiveMembership
                && (DateTime.Now < a.EndDate || a.EndDate == null)).ToList();
            List<UserNotificationDto> userNotificationsDTO = new List<UserNotificationDto>();

            if (cityHead != null)
            {
                userNotificationsDTO.Add(new UserNotificationDto
                {
                    Message = message,
                    NotificationTypeId = 1,
                    OwnerUserId = cityHead.UserId,
                    SenderLink = senderLink,
                    SenderName = "Переглянути"
                });
            }
            if (cityHeadDeputy != null)
            {
                userNotificationsDTO.Add(new UserNotificationDto
                {
                    Message = message,
                    NotificationTypeId = 1,
                    OwnerUserId = cityHeadDeputy.UserId,
                    SenderLink = senderLink,
                    SenderName = "Переглянути"
                });
            }
            if (cityReferentsUPS.Count != 0)
            {
                foreach (var referent in cityReferentsUPS)
                {
                    userNotificationsDTO.Add(new UserNotificationDto
                    {
                        Message = message,
                        NotificationTypeId = 1,
                        OwnerUserId = referent.UserId,
                        SenderLink = senderLink,
                        SenderName = "Переглянути"
                    });
                }
            }
            if (cityReferentsUSP.Count != 0)
            {
                foreach (var referent in cityReferentsUSP)
                {
                    userNotificationsDTO.Add(new UserNotificationDto
                    {
                        Message = message,
                        NotificationTypeId = 1,
                        OwnerUserId = referent.UserId,
                        SenderLink = senderLink,
                        SenderName = "Переглянути"
                    });
                }
            }
            if (cityReferentsOfActiveMembership.Count != 0)
            {
                foreach (var referent in cityReferentsOfActiveMembership)
                {
                    userNotificationsDTO.Add(new UserNotificationDto
                    {
                        Message = message,
                        NotificationTypeId = 1,
                        OwnerUserId = referent.UserId,
                        SenderLink = senderLink,
                        SenderName = "Переглянути"
                    });
                }
            }
            await _notificationService.AddListUserNotificationAsync(userNotificationsDTO);
        }

        public async Task RemoveAdminRolesByUserIdAsync(string userId)
        {
            var roles = await _repositoryWrapper.CityAdministration.GetAllAsync(a => a.UserId == userId && a.Status);
            foreach (var role in roles)
            {
                await RemoveAdministratorAsync(role.ID);
            }
        }

        private async Task SendEmailCityApproveStatusAsync(string email, string userId, DataAccess.Entities.City city, bool isApproved)
        {
            var cityUrl = _hostURLService.GetCitiesURL(city.ID);
            var emailContent = isApproved
                ? await _emailContentService.GetCityApproveEmailAsync(userId, cityUrl, city.Name)
                : await _emailContentService.GetCityExcludeEmailAsync(userId, cityUrl, city.Name);
            await _emailSendingService.SendEmailAsync(email, emailContent.Subject, emailContent.Message, emailContent.Title);
        }

        private async Task SendEmailRemoveCityFollowerAsync(string email, DataAccess.Entities.City city, string comment)
        {
            var cityUrl = _hostURLService.GetCitiesURL(city.ID);
            var emailContent = _emailContentService.GetCityRemoveFollowerEmail(cityUrl, city.Name, comment);
            await _emailSendingService.SendEmailAsync(email, emailContent.Subject, emailContent.Message,
                emailContent.Title);
        }

        private async Task<bool> CheckUserHasOneSecretaryTypeForCityAsync(CityAdministration admin)
        {
            int secretaryAdminTypesCount = 0;
            var userAdminTypes = await GetAdministrationsOfUserAsync(admin.UserId);
            foreach (CityAdministrationDto userAdminType in userAdminTypes)
            {
                var secretaryCheck = userAdminType.AdminType.AdminTypeName switch
                {
                    Roles.CityHead => Roles.CityHead,
                    Roles.CityHeadDeputy => Roles.CityHeadDeputy,
                    Roles.CityReferentUPS => Roles.CityReferentUPS,
                    Roles.CityReferentUSP => Roles.CityReferentUSP,
                    Roles.CityReferentOfActiveMembership => Roles.CityReferentOfActiveMembership,
                    _ => Roles.CitySecretary,
                };
                if (secretaryCheck == Roles.CitySecretary) secretaryAdminTypesCount++;
            }
            if (secretaryAdminTypesCount > 1) return false;
            return true;
        }
    }
}
