using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL
{
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

        public async Task<DecisionWrapperDTO> CreateDecisionAsync()
        {
            DecisionWrapperDTO decisionWrapperDto = null;
            try
            {
                decisionWrapperDto = new DecisionWrapperDTO
                {
                    Decision = new DecisionDTO(),
                    DecisionTargets = await GetDecisionTargetListAsync()
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return decisionWrapperDto;
        }

        public async Task<IEnumerable<DecisionWrapperDTO>> GetDecisionListAsync()
        {
           return await GetDecisionAsync();
        }

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

        public async Task<int> SaveDecisionAsync(DecisionWrapperDTO decision)
        {
            try
            {
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

        public async Task<string> DownloadDecisionFileFromBlobAsync(string fileName)
        {
            return await _decisionBlobStorage.GetBlobBase64Async(fileName);
        }

        private async Task UploadFileToBlobAsync(string base64, string fileName)
        {
            await _decisionBlobStorage.UploadBlobForBase64Async(base64, fileName);
        }

        public async Task<IEnumerable<OrganizationDTO>> GetOrganizationListAsync()
        {
            return _mapper.Map<IEnumerable<OrganizationDTO>>((await _repoWrapper.Organization.GetAllAsync()));
        }

        public async Task<IEnumerable<DecisionTargetDTO>> GetDecisionTargetListAsync()
        {
            return _mapper.Map<IEnumerable<DecisionTargetDTO>>((await _repoWrapper.DecesionTarget.GetAllAsync()));
        }

        public IEnumerable<SelectListItem> GetDecisionStatusTypes()
        {
            return _decisionVMCreator.GetDecesionStatusTypes();
        }

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


    }
}