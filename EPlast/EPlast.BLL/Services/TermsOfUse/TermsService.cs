using AutoMapper;
using EPlast.BLL.DTO.Terms;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Terms;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace EPlast.BLL.Services.TermsOfUse
{
    public class TermsService : ITermsService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public TermsService(IRepositoryWrapper repositoryWrapper, IMapper mapper, UserManager<User> userManager)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task<TermsDTO> GetFirstRecordAsync()
        {
            var terms = _mapper.Map<TermsDTO>(await _repoWrapper.TermsOfUse.GetFirstAsync());
            return terms;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetAllUsersIdAsync(User user)
        {
            await CheckIfAdminAsync(user);
            var allUsersId = _mapper.Map<IEnumerable<UserProfile>, IEnumerable<UserProfileDTO>>(await _repoWrapper.UserProfile.GetAllAsync())
                .Select(c => c.UserID);
            return allUsersId;
        }

        /// <inheritdoc />
        public async Task ChangeTermsAsync(TermsDTO termsDTO, User user)
        { 
            await CheckIfAdminAsync(user);
            var terms = await _repoWrapper.TermsOfUse.GetFirstAsync(x => x.TermsId == termsDTO.TermsId);
            terms.TermsTitle = termsDTO.TermsTitle;
            terms.TermsText = termsDTO.TermsText;
            terms.DatePublication = termsDTO.DatePublication;
            _repoWrapper.TermsOfUse.Update(terms);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task AddTermsAsync(TermsDTO termsDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var terms = _mapper.Map<TermsDTO, Terms>(termsDTO);
            await _repoWrapper.TermsOfUse.CreateAsync(terms);
            await _repoWrapper.SaveAsync();
        }

        /// <summary>
        /// Check if user is admin
        /// </summary>
        /// <param name="user">The id of the user</param>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to edit terms</exception>
        public async Task CheckIfAdminAsync(User user)
        {
            if (!(await _userManager.GetRolesAsync(user)).Contains(Roles.Admin))
                throw new UnauthorizedAccessException();
        }
    }
}