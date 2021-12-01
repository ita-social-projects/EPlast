using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.City
{
    public class CityParticipantsService : ICityParticipantsService
    {
        private readonly IAdminTypeService _adminTypeService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly IEmailContentService _emailContentService;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ICityService _cityService;
        private readonly UserManager<User> _userManager;

        public CityParticipantsService(IRepositoryWrapper repositoryWrapper,
                                       IMapper mapper,
                                       UserManager<User> userManager,
                                       IAdminTypeService adminTypeService,
                                       IEmailSendingService emailSendingService,
                                       ICityService cityService,
                                       IEmailContentService emailContentService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
            _adminTypeService = adminTypeService;
            _emailSendingService = emailSendingService;
            _cityService = cityService;
            _emailContentService = emailContentService;
        }

        /// <inheritdoc />
        public async Task<CityAdministrationDTO> AddAdministratorAsync(CityAdministrationDTO adminDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);
            var headType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.CityHead);
            var headDeputyType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.CityHeadDeputy);
            adminDTO.Status = DateTime.Today < adminDTO.EndDate || adminDTO.EndDate == null;
            var newAdmin = new CityAdministration()
            {
                StartDate = adminDTO.StartDate ?? DateTime.Now,
                EndDate = adminDTO.EndDate,
                AdminTypeId = adminType.ID,
                CityId = adminDTO.CityId,
                UserId = adminDTO.UserId,
                Status = adminDTO.Status
            };

            var user = await _userManager.FindByIdAsync(adminDTO.UserId);
            string role;
            switch (adminType.AdminTypeName)
            {
                case Roles.CityHead:
                    role = Roles.CityHead;
                    break;
                case Roles.CityHeadDeputy:
                    role = Roles.CityHeadDeputy;
                    break;
                default:
                    role = Roles.CitySecretary;
                    break;
            }
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

            return adminDTO;
        }

        /// <inheritdoc />
        public async Task<CityMembersDTO> AddFollowerAsync(int cityId, string userId)
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
            var regionId = await _cityService.GetByIdAsync(cityId);
            var regionAdministrations =
                await _repositoryWrapper.RegionAdministration.GetAllAsync(d =>
                    d.UserId == userId && d.Status && d.RegionId != regionId.RegionId);
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

            if (await _userManager.IsInRoleAsync(cityMember.User, Roles.RegisteredUser))
            {
                await SendEmailCityAdminAboutNewFollowerAsync(cityMember.CityId, cityMember.User);
            }

            return _mapper.Map<CityMembers, CityMembersDTO>(cityMember);
        }

        /// <inheritdoc />
        public async Task<CityMembersDTO> AddFollowerAsync(int cityId, User user)
        {
            return await AddFollowerAsync(cityId, await _userManager.GetUserIdAsync(user));
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
        public async Task<CityAdministrationDTO> EditAdministratorAsync(CityAdministrationDTO adminDTO)
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
        public async Task<IEnumerable<CityAdministrationDTO>> GetAdministrationByIdAsync(int cityId)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration.GetAllAsync(
                predicate: x => x.CityId == cityId,
                include: x => x.Include(q => q.User).
                     Include(q => q.AdminType));

            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(cityAdministration);
        }

        public async Task<IEnumerable<CityAdministrationDTO>> GetAdministrationsOfUserAsync(string UserId)
        {
            var admins = await _repositoryWrapper.CityAdministration.GetAllAsync(a => a.UserId == UserId && a.Status,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType).Include(a => a.City)
                 );

            foreach (var admin in admins)
            {
                if (admin.City != null)
                {
                    admin.City.CityAdministration = null;
                }
            }

            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(admins);
        }

        public async Task<IEnumerable<CityAdministrationStatusDTO>> GetAdministrationStatuses(string UserId)
        {
            var cityAdmins = await _repositoryWrapper.CityAdministration.GetAllAsync(a => a.UserId == UserId && !a.Status,
                             include:
                             source => source.Include(c => c.User).Include(c => c.AdminType).Include(c => c.City)
                             );
            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationStatusDTO>>(cityAdmins);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CityMembersDTO>> GetMembersByCityIdAsync(int cityId)
        {
            var cityMembers = await _repositoryWrapper.CityMembers.GetAllAsync(
                    predicate: c => c.CityId == cityId && c.EndDate == null,
                    include: source => source
                        .Include(c => c.User));

            return _mapper.Map<IEnumerable<CityMembers>, IEnumerable<CityMembersDTO>>(cityMembers);
        }

        public async Task<IEnumerable<CityAdministrationDTO>> GetPreviousAdministrationsOfUserAsync(string UserId)
        {
            var admins = await _repositoryWrapper.CityAdministration.GetAllAsync(a => a.UserId == UserId && !a.Status,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType).Include(a => a.City)
                 );

            foreach (var admin in admins)
            {
                admin.City.CityAdministration = null;
            }

            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(admins).Reverse();
        }

        /// <inheritdoc />
        public async Task RemoveAdministratorAsync(int adminId)
        {
            var admin = await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(u => u.ID == adminId);
            admin.EndDate = DateTime.Now;
            admin.Status = false;

            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(admin.AdminTypeId);
            var user = await _userManager.FindByIdAsync(admin.UserId);
            string role;
            switch (adminType.AdminTypeName)
            {
                case Roles.CityHead:
                    role = Roles.CityHead;
                    break;
                case Roles.CityHeadDeputy:
                    role = Roles.CityHeadDeputy;
                    break;
                default:
                    role = Roles.CitySecretary;
                    break;
            }
            await _userManager.RemoveFromRoleAsync(user, role);

            _repositoryWrapper.CityAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task RemoveFollowerAsync(int followerId)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.ID == followerId,
                    m => m.Include(u => u.User)
                        .Include(u => u.City));

            _repositoryWrapper.CityMembers.Delete(cityMember);
            await _repositoryWrapper.SaveAsync();

            await SendEmailRemoveCityFollowerAsync(cityMember.User.Email, cityMember.City);
        }

        public async Task RemoveMemberAsync(CityMembers member)
        {
            _repositoryWrapper.CityMembers.Delete(member);
            await _repositoryWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<CityMembersDTO> ToggleApproveStatusAsync(int memberId)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.ID == memberId,
                                        m => m.Include(u => u.User)
                                              .Include(u => u.City));
            cityMember.IsApproved = !cityMember.IsApproved; 
            _repositoryWrapper.CityMembers.Update(cityMember);
            await _repositoryWrapper.SaveAsync();
            await ChangeMembershipDatesByApprove(cityMember.UserId, cityMember.IsApproved);
            var cityMemberDto = _mapper.Map<CityMembers, CityMembersDTO>(cityMember);
            var user = await _userManager.FindByIdAsync(cityMember.UserId);
            if (cityMember.IsApproved && await _userManager.IsInRoleAsync(user, Roles.RegisteredUser))
            {
                await GiveUserSupporterRole(user);
                cityMemberDto.WasInRegisteredUserRole = true;
            }
            await SendEmailCityApproveStatusAsync(cityMember.User.Email, cityMember.UserId, cityMember.City, cityMember.IsApproved);
            return cityMemberDto;
        }

        public async Task<bool> IsUserApproved(int userId)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.ID == userId,
                                        m => m.Include(u => u.User)
                                              .Include(u => u.City));
            return cityMember.IsApproved;
        }

        public async Task<string> CityOfApprovedMember(string memberId)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.UserId == memberId,
                                        m => m.Include(u => u.City));

            if (cityMember == null)
                return null;
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

            newAdmin.Status = false;
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
            var emailContent = await _emailContentService.GetCityAdminAboutNewFollowerEmailAsync(user.Id,
                user.FirstName, user.LastName, false);
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
        }

        private async Task SendEmailCityApproveStatusAsync(string email, string userId, DataAccess.Entities.City city, bool isApproved)
        {
            var cityUrl = _repositoryWrapper.GetCitiesUrl + city.ID;
            var emailContent = isApproved
                ? await _emailContentService.GetCityApproveEmailAsync(userId, cityUrl, city.Name)
                : await _emailContentService.GetCityExcludeEmailAsync(userId, cityUrl, city.Name);
            await _emailSendingService.SendEmailAsync(email, emailContent.Subject, emailContent.Message, emailContent.Title);
        }

        private async Task SendEmailRemoveCityFollowerAsync(string email, DataAccess.Entities.City city)
        {
            var cityUrl = _repositoryWrapper.GetCitiesUrl + city.ID;
            var emailContent = _emailContentService.GetCityRemoveFollowerEmail(cityUrl, city.Name);
            await _emailSendingService.SendEmailAsync(email, emailContent.Subject, emailContent.Message,
                emailContent.Title);
        }
    }
}
