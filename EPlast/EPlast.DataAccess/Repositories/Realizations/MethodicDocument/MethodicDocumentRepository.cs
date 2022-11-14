using EPlast.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.DataAccess.Repositories
{
    public class MethodicDocumentRepository : RepositoryBase<MethodicDocument>, IMethodicDocumentRepository
    {
        public MethodicDocumentRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<MethodicDocumentTableObject> GetMethodicDocuments(string searchData, int page, int pageSize,
            string status)
        {
            if (string.IsNullOrEmpty(searchData))
            {
                searchData = "";
            }

            searchData = searchData?.ToLower();
            var getDate = string.Join("-", searchData?.Split(".").Reverse()) ?? "";

            var found = EPlastDBContext.Set<MethodicDocument>()
                .Include(doc => doc.Organization)
                .Where(doc => doc.Type == status)
                .Where(doc =>
                    string.IsNullOrWhiteSpace(searchData)
                    || doc.ID.ToString().Contains(searchData)
                    || doc.Name.ToLower().Contains(searchData)
                    || doc.Organization.OrganizationName.ToLower().Contains(searchData)
                    || doc.Description.ToLower().Contains(searchData)
                    || doc.Date.ToString().Contains(getDate)
                );

            var selected = found.Select(doc => new MethodicDocumentTableObject()
            {
                ID = doc.ID,
                Name = doc.Name,
                Description = doc.Description,
                Date = doc.Date,
                GoverningBody = doc.Organization.OrganizationName,
                Type = doc.Type,
                FileName = doc.FileName,
                Count = found.Count(),
                Total = EPlastDBContext.Set<MethodicDocument>().Count(doc => doc.Type == status)
            });

            var items = selected
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToList();

            return items;

        }
    }
}
