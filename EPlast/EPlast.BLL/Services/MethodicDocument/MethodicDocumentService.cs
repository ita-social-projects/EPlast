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
using EPlast.BLL.ExtensionMethods;

namespace EPlast.BLL.Services
{
    public class MethodicDocumentService : IMethodicDocumentService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMethodicDocumentBlobStorageRepository _metodicDocsBlobStorage;
        private readonly IUniqueIdService _uniqueId;


        public MethodicDocumentService(IRepositoryWrapper repoWrapper,
        IMapper mapper,
        IMethodicDocumentBlobStorageRepository methodicDocumentBlobStorage,
        IUniqueIdService uniqueId)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _metodicDocsBlobStorage = methodicDocumentBlobStorage;
            _uniqueId = uniqueId;
        }

        public async Task<MethodicDocumentDTO> GetMethodicDocumentAsync(int documentId)
        {
            return _mapper.Map<MethodicDocumentDTO>(await _repoWrapper.MethodicDocument
                .GetFirstAsync(x => x.ID == documentId, include: dec =>
                dec.Include(d => d.Organization)));
        }

        public async Task<IEnumerable<MethodicDocumentWraperDTO>> GetMethodicDocumentListAsync()
        {
            return await GetMethodicDocumentAsync();
        }

        public async Task ChangeMethodicDocumentAsync(MethodicDocumentDTO documentDto)
        {
            MethodicDocument document = await _repoWrapper.MethodicDocument.GetFirstAsync(x => x.ID == documentDto.ID);
            document.Name = documentDto.Name;
            document.Description = documentDto.Description;
            _repoWrapper.MethodicDocument.Update(document);
            await _repoWrapper.SaveAsync();
        }

        public MethodicDocumentWraperDTO CreateMethodicDocument()
        {
            return new MethodicDocumentWraperDTO
            {
                MethodicDocument = new MethodicDocumentDTO()
            };
        }

        public async Task DeleteMethodicDocumentAsync(int id)
        {
            var document = (await _repoWrapper.MethodicDocument.GetFirstAsync(d => d.ID == id));
            if (document == null)
                throw new ArgumentNullException($"Document with {id} id not found");
            _repoWrapper.MethodicDocument.Delete(document);
            if (document.FileName != null)
                await _metodicDocsBlobStorage.DeleteBlobAsync(document.FileName);
            await _repoWrapper.SaveAsync();
        }

        public async Task<string> DownloadMethodicDocumentFileFromBlobAsync(string fileName)
        {
            return await _metodicDocsBlobStorage.GetBlobBase64Async(fileName);
        }

        public async Task<OrganizationDTO> GetMethodicDocumentOrganizationAsync(OrganizationDTO organization)
        {
            return _mapper.Map<OrganizationDTO>(string.IsNullOrEmpty(organization.OrganizationName)
                   ? await _repoWrapper.Organization.GetFirstAsync(x => x.ID == organization.ID)
                   : await _repoWrapper.Organization.GetFirstAsync(x => x.OrganizationName.Equals(organization.OrganizationName)));
        }

        public IEnumerable<SelectListItem> GetMethodicDocumentTypes()
        {
           return (from Enum MethodicDocumentType in Enum.GetValues(typeof(MethodicDocumentTypeDTO))
             select new SelectListItem
             {
                 Value = MethodicDocumentType.ToString(),
                 Text = MethodicDocumentType.GetDescription()
             }).ToList();
        }

        public async Task<IEnumerable<OrganizationDTO>> GetOrganizationListAsync()
        {
            return _mapper.Map<IEnumerable<OrganizationDTO>>((await _repoWrapper.Organization.GetAllAsync()));
        }

        public async Task<int> SaveMethodicDocumentAsync(MethodicDocumentWraperDTO document)
        {
            var repoDoc = _mapper.Map<MethodicDocument>(document.MethodicDocument);
            _repoWrapper.MethodicDocument.Attach(repoDoc);
            _repoWrapper.MethodicDocument.Create(repoDoc);
            if (document.FileAsBase64 != null)
            {
                repoDoc.FileName = $"{_uniqueId.GetUniqueId()}{repoDoc.FileName}";
                await UploadFileToBlobAsync(document.FileAsBase64, repoDoc.FileName);
            }
            await _repoWrapper.SaveAsync();

            return document.MethodicDocument.ID;
        }

        private async Task<IEnumerable<MethodicDocumentWraperDTO>> GetMethodicDocumentAsync()
        {
            IEnumerable<MethodicDocument> methodicDocument = await _repoWrapper.MethodicDocument.GetAllAsync(include: dec =>
                dec.Include(d => d.Organization));

            return _mapper
                .Map<IEnumerable<MethodicDocumentDTO>>(methodicDocument)
                    .Select(methodicDocument => new MethodicDocumentWraperDTO { MethodicDocument = methodicDocument });
        }
        private async Task UploadFileToBlobAsync(string base64, string fileName)
        {
            await _metodicDocsBlobStorage.UploadBlobForBase64Async(base64, fileName);
        }

    }
}
