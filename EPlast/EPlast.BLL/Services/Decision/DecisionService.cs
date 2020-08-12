using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    /// <inheritdoc />
    public class DecisionService : IDecisionService
    {

        private readonly IDecisionVmInitializer _decisionVMCreator;
        private readonly IMapper _mapper;
        private readonly ILoggerService<DecisionService> _logger;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IDecisionBlobStorageRepository _decisionBlobStorage;

        public DecisionService(IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IDecisionVmInitializer decisionVMCreator,
            ILoggerService<DecisionService> logger,
            IDecisionBlobStorageRepository decisionBlobStorage)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
            _decisionVMCreator = decisionVMCreator;
            _decisionBlobStorage = decisionBlobStorage;
        }
        /// <inheritdoc />
        public async Task<DecisionDTO> GetDecisionAsync(int decisionId)
        {
            DecisionDTO decision = null;
            try
            {
                decision = _mapper.Map<DecisionDTO>(await _repoWrapper.Decesion.GetFirstAsync(x => x.ID == decisionId, include: dec =>
                dec.Include(d => d.DecesionTarget).Include(d => d.Organization)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return decision;
        }
        /// <inheritdoc />
        public DecisionWrapperDTO CreateDecision()
        {
            DecisionWrapperDTO decisionWrapperDto = null;
            try
            {
                decisionWrapperDto = new DecisionWrapperDTO
                {
                    Decision = new DecisionDTO()
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return decisionWrapperDto;
        }
        /// <inheritdoc />
        public async Task<IEnumerable<DecisionWrapperDTO>> GetDecisionListAsync()
        {
           return await GetDecisionAsync();
        }
        /// <inheritdoc />
        public async Task<bool> ChangeDecisionAsync(DecisionDTO decisionDto)
        {
            Decesion decision = null;
            try
            {
                decision = await _repoWrapper.Decesion.GetFirstAsync(x => x.ID == decisionDto.ID);
                decision.Name = decisionDto.Name;
                decision.Description = decisionDto.Description;
                _repoWrapper.Decesion.Update(decision);
                await _repoWrapper.SaveAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return decision != null;
        }
        /// <inheritdoc />
        public async Task<int> SaveDecisionAsync(DecisionWrapperDTO decision)
        {
            try
            {
                decision.Decision.DecisionTarget = await CreateDecisionTargetAsync(decision.Decision.DecisionTarget.TargetName);
                var repoDecision = _mapper.Map<Decesion>(decision.Decision);
                _repoWrapper.Decesion.Attach(repoDecision);
                _repoWrapper.Decesion.Create(repoDecision);
                if (decision.FileAsBase64 != null)
                {
                    repoDecision.FileName = Guid.NewGuid() + repoDecision.FileName;
                    await UploadFileToBlobAsync(decision.FileAsBase64, repoDecision.FileName);
                }
                await _repoWrapper.SaveAsync();

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return decision.Decision.ID;
        }
        /// <inheritdoc />
        public async Task<OrganizationDTO> GetDecisionOrganizationAsync(OrganizationDTO organization)
        {
            OrganizationDTO organizational = null;
            try
            {
                organizational = _mapper.Map<OrganizationDTO>(string.IsNullOrEmpty(organization.OrganizationName)
                    ? await _repoWrapper.Organization.GetFirstAsync(x => x.ID == organization.ID)
                    : await _repoWrapper.Organization.GetFirstAsync(x => x.OrganizationName.Equals(organization.OrganizationName)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return organizational;
        }
        /// <inheritdoc />
        public async Task<string> DownloadDecisionFileFromBlobAsync(string fileName)
        {
            return await _decisionBlobStorage.GetBlobBase64Async(fileName);
        }

        private async Task UploadFileToBlobAsync(string base64, string fileName)
        {
            await _decisionBlobStorage.UploadBlobForBase64Async(base64, fileName);
        }
        /// <inheritdoc />
        public async Task<IEnumerable<OrganizationDTO>> GetOrganizationListAsync()
        {
            return _mapper.Map<IEnumerable<OrganizationDTO>>((await _repoWrapper.Organization.GetAllAsync()));
        }
        /// <inheritdoc />
        public async Task<IEnumerable<DecisionTargetDTO>> GetDecisionTargetListAsync()
        {
            return _mapper.Map<IEnumerable<DecisionTargetDTO>>((await _repoWrapper.DecesionTarget.GetAllAsync()));
        }
        /// <inheritdoc />
        public IEnumerable<SelectListItem> GetDecisionStatusTypes()
        {
            return _decisionVMCreator.GetDecesionStatusTypes();
        }
        /// <inheritdoc />
        public async Task<bool> DeleteDecisionAsync(int id)
        {
            var success = false;
            try
            {
                var decision = (await _repoWrapper.Decesion.GetFirstAsync(d => d.ID == id));
                if (decision == null)
                    throw new ArgumentNullException($"Decision with {id} id not found");
                success = true;
                _repoWrapper.Decesion.Delete(decision);
                if (decision.FileName != null)
                    await _decisionBlobStorage.DeleteBlobAsync(decision.FileName);
                await _repoWrapper.SaveAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return success;
        }

        private async Task<IEnumerable<DecisionWrapperDTO>> GetDecisionAsync()
        {
            IEnumerable<Decesion> decisions = await _repoWrapper.Decesion.GetAllAsync(include: dec =>
                dec.Include(d => d.DecesionTarget).Include(d => d.Organization));

            return _mapper
                .Map<IEnumerable<DecisionDTO>>(decisions)
                    .Select(decision => new DecisionWrapperDTO { Decision = decision });
        }


        private async Task<DecisionTargetDTO> CreateDecisionTargetAsync(string DecisionTargetName)
        {
            DecisionTargetDTO decisionTargetDto = _mapper.Map <DecisionTargetDTO> (await _repoWrapper.DecesionTarget.GetFirstOrDefaultAsync(x=>x.TargetName==DecisionTargetName));
            
            if (decisionTargetDto == null)
            {
                decisionTargetDto = new DecisionTargetDTO();
                decisionTargetDto.TargetName = DecisionTargetName;
            }

            return decisionTargetDto;

        }
        

    }
}