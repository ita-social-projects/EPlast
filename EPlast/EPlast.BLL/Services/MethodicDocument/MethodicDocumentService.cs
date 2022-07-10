using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace EPlast.BLL.Services
{
    public class MethodicDocumentService : IMethodicDocumentService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMethodicDocumentBlobStorageRepository _metodicDocsBlobStorage;


        public MethodicDocumentService(IRepositoryWrapper repoWrapper,
        IMapper mapper,
        IMethodicDocumentBlobStorageRepository methodicDocumentBlobStorage
        )
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _metodicDocsBlobStorage = methodicDocumentBlobStorage;
        }

        public async Task<MethodicDocumentDto> GetMethodicDocumentAsync(int documentId)
        {
            return _mapper.Map<MethodicDocumentDto>(await _repoWrapper.MethodicDocument
                .GetFirstAsync(x => x.ID == documentId, include: dec =>
                dec.Include(d => d.Organization)));
        }

        public async Task<IEnumerable<MethodicDocumentWraperDto>> GetMethodicDocumentListAsync()
        {
            return await GetMethodicDocumentAsync();
        }

        public async Task ChangeMethodicDocumentAsync(MethodicDocumentDto documentDto)
        {
            MethodicDocument document = await _repoWrapper.MethodicDocument.GetFirstAsync(x => x.ID == documentDto.ID);
            document.Name = documentDto.Name;
            document.Description = documentDto.Description;
            _repoWrapper.MethodicDocument.Update(document);
            await _repoWrapper.SaveAsync();
        }

        public MethodicDocumentWraperDto CreateMethodicDocument()
        {
            return new MethodicDocumentWraperDto
            {
                MethodicDocument = new MethodicDocumentDto()
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

        public async Task<GoverningBodyDto> GetMethodicDocumentOrganizationAsync(GoverningBodyDto governingBody)
        {
            return _mapper.Map<GoverningBodyDto>(string.IsNullOrEmpty(governingBody.GoverningBodyName)
                   ? await _repoWrapper.GoverningBody.GetFirstAsync(x => x.ID == governingBody.Id)
                   : await _repoWrapper.GoverningBody.GetFirstAsync(x => x.OrganizationName.Equals(governingBody.GoverningBodyName)));
        }

        public IEnumerable<SelectListItem> GetMethodicDocumentTypes()
        {
            return (from Enum MethodicDocumentType in Enum.GetValues(typeof(MethodicDocumentTypeDto))
                    select new SelectListItem
                    {
                        Value = MethodicDocumentType.ToString(),
                        Text = MethodicDocumentType.GetDescription()
                    }).ToList();
        }

        public IEnumerable<MethodicDocumentTableObject> GetDocumentsForTable(string searchedData, int page, int pageSize, string status)
        {
            return _repoWrapper.MethodicDocument.GetMethodicDocuments(searchedData, page, pageSize, status);
        }


        public async Task<IEnumerable<GoverningBodyDto>> GetGoverningBodyListAsync()
        {
            return _mapper.Map<IEnumerable<GoverningBodyDto>>((await _repoWrapper.GoverningBody.GetAllAsync()));
        }

        public async Task<int> SaveMethodicDocumentAsync(MethodicDocumentWraperDto document)
        {
            var repoDoc = _mapper.Map<MethodicDocument>(document.MethodicDocument);
            _repoWrapper.MethodicDocument.Attach(repoDoc);
            _repoWrapper.MethodicDocument.Create(repoDoc);
            if (document.FileAsBase64 != null)
            {
                repoDoc.FileName = $"{Guid.NewGuid()}{repoDoc.FileName}";
                await UploadFileToBlobAsync(document.FileAsBase64, repoDoc.FileName);
            }
            await _repoWrapper.SaveAsync();

            return document.MethodicDocument.ID;
        }

        public async Task<MethodicDocumentDto> GetLastAsync()
        {
            var documents = await _repoWrapper.MethodicDocument.GetAllAsync(include: dec =>
                dec.Include(d => d.Organization));

            return _mapper.Map<MethodicDocumentDto>(documents.Last());
        }

        private async Task<IEnumerable<MethodicDocumentWraperDto>> GetMethodicDocumentAsync()
        {
            IEnumerable<MethodicDocument> methodicDocument = await _repoWrapper.MethodicDocument.GetAllAsync(include: dec =>
                dec.Include(d => d.Organization));

            return _mapper
                .Map<IEnumerable<MethodicDocumentDto>>(methodicDocument)
                    .Select(methodicDocument => new MethodicDocumentWraperDto { MethodicDocument = methodicDocument });
        }
        private async Task UploadFileToBlobAsync(string base64, string fileName)
        {
            await _metodicDocsBlobStorage.UploadBlobForBase64Async(base64, fileName);
        }

    }
}
