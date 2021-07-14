using EPlast.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EPlast.DataAccess.Repositories
{
    public class MethodicDocumentRepository : RepositoryBase<MethodicDocument>, IMethodicDocumentRepository
    {
        public MethodicDocumentRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<MethodicDocumentTableObject> GetMethodicDocuments(string searchData, int page, int pageSize, string status)
        {
            return EPlastDBContext.Set<MethodicDocumentTableObject>().FromSqlRaw(
                "dbo.getMethodicDocuments  @searchData = {0}, @PageIndex ={1}, @PageSize={2}, @Status ={3}", searchData, page,
                pageSize, status);
        }
    }
}
