using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BussinessLayer
{
    public class DecisionService : IDecisionService
    {
        private const string DecesionsDocumentFolder = @"\documents\";
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IDecisionVmInitializer _decisionVMCreator;
        private readonly IDirectoryManager _directoryManager;
        private readonly IFileManager _fileManager;
        private readonly IFileStreamManager _fileStreamManager;
        private readonly IMapper _mapper;
        private readonly ILoggerService<DecisionService> _logger;
        private readonly IRepositoryWrapper _repoWrapper;

        public DecisionService(IRepositoryWrapper repoWrapper, IHostingEnvironment appEnvironment,
            IDirectoryManager directoryManager, IFileManager fileManager,
            IFileStreamManager fileStreamManager, IMapper mapper, IDecisionVmInitializer decisionVMCreator,
            ILoggerService<DecisionService> logger)
        {
            _repoWrapper = repoWrapper;
            _appEnvironment = appEnvironment;
            _directoryManager = directoryManager;
            _fileManager = fileManager;
            _fileStreamManager = fileStreamManager;
            _logger = logger;
            _mapper = mapper;
            _decisionVMCreator = decisionVMCreator;
        }

        public async Task<DecisionDTO> GetDecisionAsync(int decisionId)
        {
            DecisionDTO decision = null;
            try
            {
                decision = _mapper.Map<DecisionDTO>(await _repoWrapper.Decesion.GetFirstAsync(x => x.ID == decisionId));
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

        public async Task<List<DecisionWrapperDTO>> GetDecisionListAsync()
        {
            List<DecisionWrapperDTO> decisionList = null;
            try
            {
                decisionList = await getDecisionListAsync();
                foreach (var decision in decisionList)
                {
                    var path = _appEnvironment.WebRootPath + DecesionsDocumentFolder + decision.Decision.ID;
                    if (!decision.Decision.HaveFile || !_directoryManager.Exists(path)) continue;
                    var files = _directoryManager.GetFiles(path);

                    if (files.Length == 0) throw new ArgumentException($"File count in '{path}' is 0");

                    decision.Filename = Path.GetFileName(files.First());
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return decisionList;
        }

        public async Task<bool> ChangeDecisionAsync(DecisionDTO decisionDto)
        {
            Decesion decision = null;
            try
            {
                decision = await _repoWrapper.Decesion.GetFirstAsync(x => x.ID == decisionDto.ID);
                decision.Name = decision.Name;
                decision.Description = decision.Description;
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
                await _repoWrapper.SaveAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }
            if (decision.Decision.HaveFile)
                await SaveDecisionFileAsync(decision);
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

        public async Task<byte[]> DownloadDecisionFileAsync(int decisionId)
        {
            if (decisionId <= 0) return null;
            MemoryStream memory = null;
            try
            {
                var path = GetDecisionFilePath(decisionId);

                DownloadDecisionFilePathCheck(path);
                var filename = _directoryManager.GetFiles(path).First();
                path = Path.Combine(path, filename);
                memory = new MemoryStream();

                using (var stream = _fileStreamManager.GenerateFileStreamManager(path, FileMode.Open))
                {
                    await _fileStreamManager.CopyToAsync(stream.GetStream(), memory);

                    if (memory.Length == 0) throw new ArgumentException("memory length is 0");
                }

                memory.Position = 0;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return memory?.ToArray();
        }

        private void DownloadDecisionFilePathCheck(string path)
        {
            if (!_directoryManager.Exists(path) || _directoryManager.GetFiles(path).Length == 0)
                throw new ArgumentException($"directory '{path}' does not exist");
        }

        public string GetContentType(int decisionId, string filename)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(Path.Combine(GetDecisionFilePath(decisionId), filename)).ToLowerInvariant();
            return types[ext];
        }

        public async Task<List<OrganizationDTO>> GetOrganizationListAsync()
        {
            return _mapper.Map<List<OrganizationDTO>>((await _repoWrapper.Organization.GetAllAsync()).ToList());
        }

        public async Task<List<DecisionTargetDTO>> GetDecisionTargetListAsync()
        {
            return _mapper.Map<List<DecisionTargetDTO>>((await _repoWrapper.DecesionTarget.GetAllAsync()).ToList());
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
                await _repoWrapper.SaveAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return success;
        }

        private async Task<List<DecisionWrapperDTO>> getDecisionListAsync()
        {
            var decisions = (await _repoWrapper.Decesion.GetAllAsync(include: dec =>
                dec.Include(d => d.DecesionTarget).Include(d => d.Organization))).ToList();
            return _mapper
                .Map<List<DecisionDTO>>(decisions)
                    .Select(decision => new DecisionWrapperDTO { Decision = decision })
                .ToList();
        }

        private string GetDecisionFilePath(int decisionId)
        {
            return Path.Combine(_appEnvironment.WebRootPath + DecesionsDocumentFolder, decisionId.ToString());
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".mp4", "video/mp4"}
            };
        }

        private async Task SaveDecisionFileAsync(DecisionWrapperDTO decision)
        {
            try
            {
                var path = _appEnvironment.WebRootPath + DecesionsDocumentFolder + decision.Decision.ID;

                _directoryManager.CreateDirectory(path);

                SaveDecisionFilePathCreateCheck(path);

                if (decision.File != null)
                {
                    path = Path.Combine(path, decision.File.FileName);

                    using (var stream = _fileStreamManager.GenerateFileStreamManager(path, FileMode.Create))
                    {
                        await _fileStreamManager.CopyToAsync(decision.File, stream.GetStream());
                        if (!_fileManager.Exists(path))
                            throw new ArgumentException($"File was not created it '{path}' directory");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }
        }

        private void SaveDecisionFilePathCreateCheck(string path)
        {
            if (!_directoryManager.Exists(path))
                throw new ArgumentException($"directory '{path}' is not exist");
        }
    }
}