using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;

namespace EPlast.DataAccess.Repositories.Contracts
{
    public interface IAnnualReportsRepository : IRepositoryBase<AnnualReport>
    {
        Task<IEnumerable<AnnualReportTableObject>> GetAnnualReportsAsync(string userId, bool isAdmin, string searchdata,
            int page, int pageSize, int sortKey, bool auth);
    }
}
