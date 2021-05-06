using AutoMapper;
using EPlast.BLL.DTO.AboutBase;
using EPlast.BLL.Interfaces.AboutBase;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.AboutBase
{
    public class AboutBaseSubsectionService : IAboutBaseSubsectionService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public AboutBaseSubsectionService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task AddSubsection(SubsectionDTO subsectionDTO)
        {
            var subsection = _mapper.Map<SubsectionDTO, Subsection>(subsectionDTO);
            await _repoWrapper.AboutBaseSubsection.CreateAsync(subsection);
            await _repoWrapper.SaveAsync();
        }

        public async Task ChangeSubsection(SubsectionDTO subsectionDTO)
        {
            var subsection = await _repoWrapper.AboutBaseSubsection.GetFirstAsync(x => x.Id == subsectionDTO.Id);
            subsection.Title = subsectionDTO.Title;
            subsection.Description = subsectionDTO.Description;
            _repoWrapper.AboutBaseSubsection.Update(subsection);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeleteSubsection(int id)
        {
            var subsection = (await _repoWrapper.AboutBaseSubsection.GetFirstAsync(subsection => subsection.Id == id));
            if (subsection == null)
                throw new ArgumentNullException($"Subsection with {id} not found");
            _repoWrapper.AboutBaseSubsection.Delete(subsection);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<SubsectionDTO>> GetAllSubsectionAsync()
        {
            return _mapper.Map<IEnumerable<Subsection>, IEnumerable<SubsectionDTO>>(await _repoWrapper.AboutBaseSubsection.GetAllAsync());
        }

        public async Task<SubsectionDTO> GetSubsection(int id)
        {
            var subsection = _mapper.Map<SubsectionDTO>(await _repoWrapper.AboutBaseSubsection.GetFirstAsync(s => s.Id == id));
            return subsection;
        }
    }
}
