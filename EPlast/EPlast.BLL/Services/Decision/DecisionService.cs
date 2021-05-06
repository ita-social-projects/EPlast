using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.Decision;

namespace EPlast.BLL.Services
{
    /// <inheritdoc />
    public class DecisionService : IDecisionService
    {
        private readonly IDecisionVmInitializer _decisionVMCreator;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IDecisionBlobStorageRepository _decisionBlobStorage;
        private readonly IUniqueIdService _uniqueId;

        public DecisionService(IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IDecisionVmInitializer decisionVMCreator,
            IDecisionBlobStorageRepository decisionBlobStorage,
            IUniqueIdService uniqueId)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _decisionVMCreator = decisionVMCreator;
            _decisionBlobStorage = decisionBlobStorage;
            _uniqueId = uniqueId;
        }

        /// <inheritdoc />
        public async Task<DecisionDTO> GetDecisionAsync(int decisionId)
        {
            return _mapper.Map<DecisionDTO>(await _repoWrapper.Decesion
                .GetFirstAsync(x => x.ID == decisionId, include: dec =>
                dec.Include(d => d.DecesionTarget).Include(d => d.Organization)));
        }

        /// <inheritdoc />
        public DecisionWrapperDTO CreateDecision()
        {
            return new DecisionWrapperDTO
            {
                Decision = new DecisionDTO()
            };
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DecisionWrapperDTO>> GetDecisionListAsync()
        {
            return await GetDecisionAsync();
        }

        public IEnumerable<DecisionTableObject> GetDecisionsForTable(string searchedData, int page, int pageSize)
        {
            return _repoWrapper.Decesion.GetDecisions(searchedData, page, pageSize);
        }

        /// <inheritdoc />
        public async Task ChangeDecisionAsync(DecisionDTO decisionDto)
        {
            Decesion decision = await _repoWrapper.Decesion.GetFirstAsync(x => x.ID == decisionDto.ID);
            decision.Name = decisionDto.Name;
            decision.Description = decisionDto.Description;
            _repoWrapper.Decesion.Update(decision);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<int> SaveDecisionAsync(DecisionWrapperDTO decision)
        {
            decision.Decision.DecisionTarget = await CreateDecisionTargetAsync(decision.Decision.DecisionTarget.TargetName);
            var repoDecision = _mapper.Map<Decesion>(decision.Decision);
            _repoWrapper.Decesion.Attach(repoDecision);
            _repoWrapper.Decesion.Create(repoDecision);
            if (decision.FileAsBase64 != null)
            {
                repoDecision.FileName = $"{_uniqueId.GetUniqueId()}{repoDecision.FileName}";
                await UploadFileToBlobAsync(decision.FileAsBase64, repoDecision.FileName);
            }
            await _repoWrapper.SaveAsync();

            return decision.Decision.ID;
        }

        /// <inheritdoc />
        public async Task<GoverningBodyDTO> GetDecisionOrganizationAsync(GoverningBodyDTO governingBody)
        {
            return _mapper.Map<GoverningBodyDTO>(string.IsNullOrEmpty(governingBody.GoverningBodyName)
                    ? await _repoWrapper.GoverningBody.GetFirstAsync(x => x.ID == governingBody.Id)
                    : await _repoWrapper.GoverningBody.GetFirstAsync(x => x.OrganizationName.Equals(governingBody.GoverningBodyName)));
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
        public async Task<IEnumerable<GoverningBodyDTO>> GetGoverningBodyListAsync()
        {
            return _mapper.Map<IEnumerable<GoverningBodyDTO>>((await _repoWrapper.GoverningBody.GetAllAsync()));
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
        public async Task DeleteDecisionAsync(int id)
        {
            var decision = (await _repoWrapper.Decesion.GetFirstAsync(d => d.ID == id));
            if (decision == null)
                throw new ArgumentNullException($"Decision with {id} id not found");
            _repoWrapper.Decesion.Delete(decision);
            if (decision.FileName != null)
                await _decisionBlobStorage.DeleteBlobAsync(decision.FileName);
            await _repoWrapper.SaveAsync();
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
            DecisionTargetDTO decisionTargetDto = _mapper.Map<DecisionTargetDTO>(await _repoWrapper.DecesionTarget.GetFirstOrDefaultAsync(x => x.TargetName == DecisionTargetName));

            if (decisionTargetDto == null)
            {
                decisionTargetDto = new DecisionTargetDTO();
                decisionTargetDto.TargetName = DecisionTargetName;
            }

            return decisionTargetDto;
        }
    }
}
