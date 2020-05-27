﻿using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BussinessLayer.Services;
using EPlast.BussinessLayer.Services.Interfaces;

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
            IFileStreamManager fileStreamManager, IMapper mapper, IDecisionVmInitializer decisionVMCreator, ILoggerService<DecisionService> logger)
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

        public DecisionDTO GetDecision(int decisionId)
        {
            DecisionDTO decision = null;
            try
            {
                decision = _mapper.Map<DecisionDTO>(_repoWrapper.Decesion.FindByCondition(x => x.ID == decisionId)
                    .First());
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return decision;
        }

        public DecisionWrapperDTO CreateDecision()
        {
            DecisionWrapperDTO decisionWrapperDto = null;
            try
            {
                decisionWrapperDto = new DecisionWrapperDTO
                {
                    Decision = new DecisionDTO(),
                    DecisionTargets = GetDecisionTargetList()
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return decisionWrapperDto;
        }

        public List<DecisionWrapperDTO> GetDecisionList()
        {
            List<DecisionWrapperDTO> decisionList = null;
            try
            {
                decisionList = GetDecisionListAsync();
                foreach (var decesion in decisionList)
                {
                    var path = _appEnvironment.WebRootPath + DecesionsDocumentFolder + decesion.Decision.ID;
                    if (!decesion.Decision.HaveFile || !_directoryManager.Exists(path)) continue;
                    var files = _directoryManager.GetFiles(path);

                    if (files.Length == 0) throw new ArgumentException($"File count in '{path}' is 0");

                    decesion.Filename = Path.GetFileName(files.First());
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return decisionList;
        }

        public bool ChangeDecision(DecisionDTO decision)
        {
            Decesion decesion = null;
            try
            {
                decesion = _repoWrapper.Decesion.FindByCondition(x => x.ID == decision.ID).First();
                decesion.Name = decision.Name;
                decesion.Description = decision.Description;
                _repoWrapper.Decesion.Update(decesion);
                _repoWrapper.Save();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return decesion != null;
        }

        public Task<bool> SaveDecisionAsync(DecisionWrapperDTO decision)
        {
            try
            {
                _repoWrapper.Decesion.Attach(_mapper.Map<DecisionDTO, Decesion>(decision.Decision));
                _repoWrapper.Decesion.Create(_mapper.Map<DecisionDTO, Decesion>(decision.Decision));
                _repoWrapper.Save();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return !decision.Decision.HaveFile ? Task.FromResult(true) : SaveDecisionFile(decision);
        }

        public OrganizationDTO GetDecisionOrganization(int decisionId)
        {
            OrganizationDTO organization = null;
            try
            {
                organization = _mapper.Map<OrganizationDTO>(_repoWrapper.Organization
                    .FindByCondition(x => x.ID == decisionId).Select(x => x.OrganizationName).First());
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return organization;
        }

        public async Task<byte[]> DownloadDecisionFileAsync(int decisionId)
        {
            if (decisionId <= 0) return null;
            MemoryStream memory = null;
            try
            {
                var path = GetDecisionFilePath(decisionId);

                if (!_directoryManager.Exists(path) || _directoryManager.GetFiles(path).Length == 0)
                    throw new ArgumentException($"directory '{path}' is not exist");

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

        public string GetContentType(int decisionId, string filename)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(Path.Combine(GetDecisionFilePath(decisionId), filename)).ToLowerInvariant();
            return types[ext];
        }

        public List<OrganizationDTO> GetOrganizationList()
        {
            return _mapper.Map<List<OrganizationDTO>>(_repoWrapper.Organization.FindAll().ToList());
        }

        public List<DecisionTargetDTO> GetDecisionTargetList()
        {
            return _mapper.Map<List<DecisionTargetDTO>>(_repoWrapper.DecesionTarget.FindAll().ToList());
        }

        public IEnumerable<SelectListItem> GetDecisionStatusTypes()
        {
            return _decisionVMCreator.GetDecesionStatusTypes();
        }

        public bool DeleteDecision(int decisionId)
        {
            bool success = false;
            try
            {
                var decision = _repoWrapper.Decesion.FindByCondition(d => d.ID == decisionId).First();
                success = decision == null
                    ? throw new ArgumentNullException($"Decision with {decisionId} id not found")
                    : true;
                _repoWrapper.Decesion.Delete(decision);
                _repoWrapper.Save();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }

            return success;
        }

        private List<DecisionWrapperDTO> GetDecisionListAsync()
        {
            return _mapper
                .Map<List<DecisionDTO>>(_repoWrapper.Decesion
                    .Include(x => x.DecesionTarget, x => x.Organization)
                    .ToList())
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

        private async Task<bool> SaveDecisionFile(DecisionWrapperDTO decision)
        {
            try
            {
                var path = _appEnvironment.WebRootPath + DecesionsDocumentFolder + decision.Decision.ID;

                _directoryManager.CreateDirectory(path);

                if (!_directoryManager.Exists(path))
                    throw new ArgumentException($"directory '{path}' is not exist");

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

            return true;
        }
    }
}