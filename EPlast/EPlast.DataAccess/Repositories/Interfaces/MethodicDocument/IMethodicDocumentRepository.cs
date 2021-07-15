using EPlast.DataAccess.Entities;
using System.Collections.Generic;

namespace EPlast.DataAccess.Repositories
{
    public interface IMethodicDocumentRepository : IRepositoryBase<MethodicDocument>
    {
        IEnumerable<MethodicDocumentTableObject> GetMethodicDocuments(string searchData, int page, int pageSize, string status);
    }
}
