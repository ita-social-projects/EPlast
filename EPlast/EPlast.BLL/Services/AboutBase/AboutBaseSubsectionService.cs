using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.AboutBase;
using EPlast.BLL.Interfaces.AboutBase;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.AboutBase;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Services.AboutBase
{
    public class AboutBaseSubsectionService : IAboutBaseSubsectionService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AboutBaseSubsectionService(IRepositoryWrapper repositoryWrapper, IMapper mapper, UserManager<User> userManager)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task AddSubsection(SubsectionDto subsectionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var subsection = _mapper.Map<SubsectionDto, Subsection>(subsectionDTO);
            await _repoWrapper.AboutBaseSubsection.CreateAsync(subsection);
            await _repoWrapper.SaveAsync();
        }

        public async Task ChangeSubsection(SubsectionDto subsectionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var subsection = await _repoWrapper.AboutBaseSubsection.GetFirstAsync(x => x.Id == subsectionDTO.Id);
            subsection.Title = subsectionDTO.Title;
            subsection.Description = subsectionDTO.Description;
            _repoWrapper.AboutBaseSubsection.Update(subsection);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeleteSubsection(int id, User user)
        {
            await CheckIfAdminAsync(user);
            var subsection = (await _repoWrapper.AboutBaseSubsection.GetFirstAsync(subsection => subsection.Id == id));
            if (subsection == null)
                throw new ArgumentNullException($"Subsection with {id} not found");
            _repoWrapper.AboutBaseSubsection.Delete(subsection);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<SubsectionDto>> GetAllSubsectionAsync()
        {
            return _mapper.Map<IEnumerable<Subsection>, IEnumerable<SubsectionDto>>(await _repoWrapper.AboutBaseSubsection.GetAllAsync());
        }

        public async Task<SubsectionDto> GetSubsection(int id)
        {
            var subsection = _mapper.Map<SubsectionDto>(await _repoWrapper.AboutBaseSubsection.GetFirstAsync(s => s.Id == id));
            return subsection;
        }

        public async Task CheckIfAdminAsync(User user)
        {
            if (!(await _userManager.GetRolesAsync(user)).Contains(Roles.Admin))
                throw new UnauthorizedAccessException();
        }
    }
}
