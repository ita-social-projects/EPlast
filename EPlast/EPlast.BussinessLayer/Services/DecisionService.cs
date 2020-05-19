using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace EPlast.BussinessLayer
{
    public class DecisionService : IDecisionService
    {
        private const string DecesionsDocumentFolder = @"\documents\";
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IDirectoryManager _directoryManager;
        private readonly IFileManager _fileManager;
        private readonly IFileStreamManager _fileStreamManager;
        private readonly ILogger _logger;
        private readonly IRepositoryWrapper _repoWrapper;

        public DecisionService(IRepositoryWrapper repoWrapper, IHostingEnvironment appEnvironment,
            IDirectoryManager directoryManager,
            IFileManager fileManager, IFileStreamManager fileStreamManager, ILogger logger)
        {
            _repoWrapper = repoWrapper;
            _appEnvironment = appEnvironment;
            _directoryManager = directoryManager;
            _fileManager = fileManager;
            _fileStreamManager = fileStreamManager;
            _logger = logger;
        }

        public async Task<DecisionDto> GetDecision(int decisionId)
        {
            DecisionDto decision = null;
            try
            {
                decision = new DecisionDto
                {
                    Decesion = await Task.Run(() =>
                        _repoWrapper.Decesion.FindByCondition(x => x.ID == decisionId).First())
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
            }

            return decision;
        }

        public async Task<List<DecisionDto>> GetDecisionList()
        {
            List<DecisionDto> decisionList = null;
            try
            {
                decisionList = await GetDecisionListAsync();
                foreach (var decesion in decisionList)
                {
                    var path = _appEnvironment.WebRootPath + DecesionsDocumentFolder + decesion.Decesion.ID;
                    if (!decesion.Decesion.HaveFile || !_directoryManager.Exists(path)) continue;
                    var files = _directoryManager.GetFiles(path);

                    if (files.Length == 0) throw new ArgumentException($"File count in '{path}' is 0");

                    decesion.Filename = Path.GetFileName(files.First());
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
            }

            return decisionList;
        }

        public async Task<bool> ChangeDecision(DecisionDto decision)
        {
            Decesion decesion = null;
            try
            {
                decesion = await Task.Run(() =>
                    _repoWrapper.Decesion.FindByCondition(x => x.ID == decision.Decesion.ID).First());
                decesion.Name = decision.Decesion.Name;
                decesion.Description = decision.Decesion.Description;
                _repoWrapper.Decesion.Update(decesion);
                _repoWrapper.Save();
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
            }

            return decesion != null;
        }

        public Task<bool> SaveDecision(DecisionDto decision)
        {
            try
            {
                _repoWrapper.Decesion.Attach(decision.Decesion);
                _repoWrapper.Decesion.Create(decision.Decesion);
                _repoWrapper.Save();
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
            }

            return !decision.Decesion.HaveFile ? Task.FromResult(true) : SaveDecisionFile(decision);
        }

        public async Task<byte[]> DownloadDecisionFile(int decisionId)
        {
            if (decisionId <= 0)
                return null;
            MemoryStream memory = null;
            try
            {
                var path = Path.Combine(_appEnvironment.WebRootPath + DecesionsDocumentFolder, decisionId.ToString());

                if (!_directoryManager.Exists(path) || _directoryManager.GetFiles(path).Length == 0)
                    throw new ArgumentException($"directory '{path}' is not exist");

                var filename = _directoryManager.GetFiles(path).First();
                path = Path.Combine(path, filename);
                memory = new MemoryStream();

                using (var stream = _fileStreamManager.GenerateFileStreamManager(path, FileMode.Open))
                {
                    await _fileStreamManager.CopyToAsync(stream.GetStream(), memory);

                    if (memory.Length == 0)
                        throw new ArgumentException("memory length is 0");
                }

                memory.Position = 0;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
            }

            return memory?.ToArray();
        }

        private Task<List<DecisionDto>> GetDecisionListAsync()
        {
            return Task.Run(() => new List<DecisionDto>(
                _repoWrapper.Decesion
                    .Include(x => x.DecesionTarget, x => x.Organization)
                    .Select(decesion => new DecisionDto
                    {
                        Decesion = decesion
                    })
                    .ToList()));
        }

        public string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path)?.ToLowerInvariant();
            return types[ext ?? throw new InvalidOperationException()];
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

        private async Task<bool> SaveDecisionFile(DecisionDto decision)
        {
            try
            {
                var path = _appEnvironment.WebRootPath + DecesionsDocumentFolder + decision.Decesion.ID;

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
                _logger.LogError("Exception: {0}", e.Message);
            }

            return true;
        }
    }
}