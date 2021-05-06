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
    public class AboutBaseSectionService : IAboutBaseSectionService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public AboutBaseSectionService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task AddSection(SectionDTO sectionDTO)
        {
            var section = _mapper.Map<SectionDTO, Section>(sectionDTO);
            await _repoWrapper.AboutBaseSection.CreateAsync(section);
            await _repoWrapper.SaveAsync();
        }

        public async Task ChangeSection(SectionDTO sectionDTO)
        {
            var section = await _repoWrapper.AboutBaseSection.GetFirstAsync(x => x.Id == sectionDTO.Id);
            section.Title = sectionDTO.Title;
            _repoWrapper.AboutBaseSection.Update(section);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeleteSection(int id)
        {
            var section = (await _repoWrapper.AboutBaseSection.GetFirstAsync(section => section.Id == id));
            if (section == null)
                throw new ArgumentNullException($"Section with {id} not found");
            _repoWrapper.AboutBaseSection.Delete(section);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<SectionDTO>> GetAllSectionAsync()
        {
            return _mapper.Map<IEnumerable<Section>, IEnumerable<SectionDTO>>(await _repoWrapper.AboutBaseSection.GetAllAsync());
        }

        public async Task<SectionDTO> GetSection(int id)
        {
            var section = _mapper.Map<SectionDTO>(await _repoWrapper.AboutBaseSection.GetFirstAsync(s => s.Id == id));
            return section;
        }
    }
}
