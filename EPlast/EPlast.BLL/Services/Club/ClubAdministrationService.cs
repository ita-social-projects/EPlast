using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Club
{
    public class ClubAdministrationService : IClubAdministrationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IAdminTypeService _adminTypeService;
        private readonly UserManager<User> _userManager;

        public ClubAdministrationService(IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IAdminTypeService adminTypeService,
            UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _adminTypeService = adminTypeService;
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ClubAdministrationDTO>> GetAdministrationByIdAsync(int ClubId)
        {
            var ClubAdministration = await _repositoryWrapper.ClubAdministration.GetAllAsync(
                predicate: x => x.ClubId == ClubId,
                include: x => x.Include(q => q.User).
                     Include(q => q.AdminType));

            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDTO>>(ClubAdministration);
        }

        /// <inheritdoc />
        public async Task<ClubAdministrationDTO> AddAdministratorAsync(ClubAdministrationDTO adminDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);
            var admin = new ClubAdministration()
            {
                StartDate = adminDTO.StartDate ?? DateTime.Now,
                EndDate = adminDTO.EndDate,
                AdminTypeId = adminType.ID,
                ClubId = adminDTO.ClubId,
                UserId = adminDTO.UserId
            };

            var user = await _userManager.FindByIdAsync(adminDTO.UserId);
            var role = adminType.AdminTypeName == "Голова Куреня" ? "Голова Куреня" : "Діловод Куреня";
            await _userManager.AddToRoleAsync(user, role);

            if (role == "Голова Куреня")
            {
                await CheckClubHasHead(adminDTO.ClubId);
            }

            await _repositoryWrapper.ClubAdministration.CreateAsync(admin);
            await _repositoryWrapper.SaveAsync();
            adminDTO.ID = admin.ID;

            return adminDTO;
        }

        /// <inheritdoc />
        public async Task<ClubAdministrationDTO> EditAdministratorAsync(ClubAdministrationDTO adminDTO)
        {
            var admin = await _repositoryWrapper.ClubAdministration.GetFirstOrDefaultAsync(a => a.ID == adminDTO.ID);
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync(adminDTO.AdminType.AdminTypeName);

            if (adminType.ID == admin.AdminTypeId)
            {
                admin.StartDate = adminDTO.StartDate ?? DateTime.Now;
                admin.EndDate = adminDTO.EndDate;

                _repositoryWrapper.ClubAdministration.Update(admin);
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
        public async Task RemoveAdministratorAsync(int adminId)
        {
            var admin = await _repositoryWrapper.ClubAdministration.GetFirstOrDefaultAsync(u => u.ID == adminId);
            admin.EndDate = DateTime.Now;

            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(admin.AdminTypeId);
            var user = await _userManager.FindByIdAsync(admin.UserId);
            var role = adminType.AdminTypeName == "Голова Куреня" ? "Голова Куреня" : "Діловод Куреня";
            await _userManager.RemoveFromRoleAsync(user, role);

            _repositoryWrapper.ClubAdministration.Update(admin);
            await _repositoryWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task CheckPreviousAdministratorsToDelete()
        {
            var admins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.EndDate <= DateTime.Now);
            var ClubHeadType = await _adminTypeService.GetAdminTypeByNameAsync("Голова Куреня");

            foreach (var admin in admins)
            {
                var role = admin.AdminTypeId == ClubHeadType.ID ? "Голова Куреня" : "Діловод Куреня";

                var currentAdministration = await _repositoryWrapper.ClubAdministration
                    .GetAllAsync(a => (a.EndDate > DateTime.Now || a.EndDate == null) && a.UserId == admin.UserId);

                if (currentAdministration.All(a => (a.AdminTypeId == ClubHeadType.ID ? "Голова Куреня" : "Діловод Куреня") != role)
                    || currentAdministration.Count() == 0)
                {
                    var user = await _userManager.FindByIdAsync(admin.UserId);

                    await _userManager.RemoveFromRoleAsync(user, role);
                }
            }
        }

        public async Task<IEnumerable<ClubAdministrationDTO>> GetAdministrationsOfUserAsync(string UserId)
        {
            var admins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.UserId == UserId && a.Status==true,
                 include:
                 source => source.Include(c => c.User).Include(a => a.Club).Include(c => c.AdminType)
                 ); ;
            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDTO>>(admins);
        }


        public async Task<IEnumerable<ClubAdministrationDTO>> GetPreviousAdministrationsOfUserAsync(string UserId)
        {
            var admins = await _repositoryWrapper.ClubAdministration.GetAllAsync(a => a.UserId == UserId && a.Status == false,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType).Include(c => c.Club)
                 );
            return _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDTO>>(admins);
        }
           

        public async Task CheckClubHasHead(int clubId)
        {
            var adminType = await _adminTypeService.GetAdminTypeByNameAsync("Голова Куреня");
            var admin = await _repositoryWrapper.ClubAdministration.
                GetFirstOrDefaultAsync(a => a.AdminTypeId == adminType.ID
                    && (DateTime.Now < a.EndDate || a.EndDate == null));

            if (admin != null)
            {
                await RemoveAdministratorAsync(admin.ID);
            }

        }
    }
}
