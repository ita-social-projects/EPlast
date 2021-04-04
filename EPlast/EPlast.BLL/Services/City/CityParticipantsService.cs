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
        private readonly UserManager<User> _userManager;

        public CityParticipantsService(IRepositoryWrapper repositoryWrapper,
                                       IMapper mapper,
                                       UserManager<User> userManager,
                                       IAdminTypeService adminTypeService,
                                       IEmailSendingService emailSendingService,
                                       IEmailContentService emailContentService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
            _adminTypeService = adminTypeService;
            _emailSendingService = emailSendingService;
            _emailContentService = emailContentService;
        }

        /// <inheritdoc />
        public async Task<CityAdministrationDTO> AddAdministratorAsync(CityAdministrationDTO adminDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);
            var admin = new CityAdministration()
            {
                StartDate = adminDTO.StartDate ?? DateTime.Now,
                EndDate = adminDTO.EndDate,
                AdminTypeId = adminType.ID,
                CityId = adminDTO.CityId,
                UserId = adminDTO.UserId
            };

            var user = await _userManager.FindByIdAsync(adminDTO.UserId);
            var role = adminType.AdminTypeName == Roles.CityHead ? Roles.CityHead : Roles.CitySecretary;
            await _userManager.AddToRoleAsync(user, role);

            await CheckCityHasAdmin(adminDTO.CityId, adminType.AdminTypeName);

            await _repositoryWrapper.CityAdministration.CreateAsync(admin);
            await _repositoryWrapper.SaveAsync();
            adminDTO.ID = admin.ID;

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
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<CityMembers, CityMembersDTO>(cityMember);
        }

        /// <inheritdoc />
        public async Task<CityMembersDTO> AddFollowerAsync(int cityId, User user)
        {
            return await AddFollowerAsync(cityId, await _userManager.GetUserIdAsync(user));
        }

        /// <inheritdoc />
        public async Task CheckPreviousAdministratorsToDelete()
        {
            var admins = await _repositoryWrapper.CityAdministration.GetAllAsync(a => a.EndDate <= DateTime.Now);
            var cityHeadType = await _adminTypeService.GetAdminTypeByNameAsync(Roles.CityHead);

            foreach (var admin in admins)
            {
                var role = admin.AdminTypeId == cityHeadType.ID ? Roles.CityHead : Roles.CitySecretary;

                var currentAdministration = await _repositoryWrapper.CityAdministration
                    .GetAllAsync(a => (a.EndDate > DateTime.Now || a.EndDate == null) && a.UserId == admin.UserId);

                if (currentAdministration.All(a => (a.AdminTypeId == cityHeadType.ID ? Roles.CityHead : Roles.CitySecretary) != role)
                    || !currentAdministration.Any())
                {
                    var user = await _userManager.FindByIdAsync(admin.UserId);

                    await _userManager.RemoveFromRoleAsync(user, role);
                }
            }
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

                _repositoryWrapper.CityAdministration.Update(admin);
                await _repositoryWrapper.SaveAsync();
            }
            else
            {
                await RemoveAdministratorAsync(adminDTO.ID);
                adminDTO = await AddAdministratorAsync(adminDTO);
            }

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
            var admins = await _repositoryWrapper.CityAdministration.GetAllAsync(a => a.UserId == UserId && (a.EndDate > DateTime.Now || a.EndDate == null),
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
            var admins = await _repositoryWrapper.CityAdministration.GetAllAsync(a => a.UserId == UserId && a.EndDate < DateTime.Now,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType).Include(a => a.City)
                 );

            foreach (var admin in admins)
            {
                admin.City.CityAdministration = null;
            }

            return _mapper.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(admins);
        }

        /// <inheritdoc />
        public async Task RemoveAdministratorAsync(int adminId)
        {
            var admin = await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(u => u.ID == adminId);
            admin.EndDate = DateTime.Now;

            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(admin.AdminTypeId);
            var user = await _userManager.FindByIdAsync(admin.UserId);
            var role = adminType.AdminTypeName == Roles.CityHead ? Roles.CityHead : Roles.CitySecretary;
            await _userManager.RemoveFromRoleAsync(user, role);

            _repositoryWrapper.CityAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task RemoveFollowerAsync(int followerId)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.ID == followerId);

            _repositoryWrapper.CityMembers.Delete(cityMember);
            await _repositoryWrapper.SaveAsync();
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
            await SendEmailCityApproveAsync(cityMember.User.Email, cityMember.City, cityMember.IsApproved);
            return _mapper.Map<CityMembers, CityMembersDTO>(cityMember);
        }

        private async Task ChangeMembershipDatesByApprove(string userId, bool isApproved)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (!await _userManager.IsInRoleAsync(user, Roles.PlastMember) && user != null)
            {
                var userMembershipDates = await _repositoryWrapper.UserMembershipDates
                            .GetFirstOrDefaultAsync(umd => umd.UserId == userId);
                userMembershipDates.DateEntry = isApproved ? DateTime.Now : default;
                user.RegistredOn = DateTime.Now;
                _repositoryWrapper.UserMembershipDates.Update(userMembershipDates);
                await _repositoryWrapper.SaveAsync();
            }
        }

        private async Task CheckCityHasAdmin(int cityId, string adminTypeName)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminTypeName);
            var admin = await _repositoryWrapper.CityAdministration.
                GetFirstOrDefaultAsync(a => a.AdminTypeId == adminType.ID
                    && (DateTime.Now < a.EndDate || a.EndDate == null) && a.CityId == cityId);

            if (admin != null)
            {
                await RemoveAdministratorAsync(admin.ID);
            }
        }

        private async Task SendEmailCityApproveAsync(string email, DataAccess.Entities.City city, bool isApproved)
        {
            var cityUrl = _repositoryWrapper.GetCitiesUrl + city.ID;
            var emailContent = _emailContentService.GetCityApproveEmail(cityUrl, city.Name, isApproved);
            await _emailSendingService.SendEmailAsync(email, emailContent.Subject, emailContent.Message, emailContent.Title);
        }
    }
}
