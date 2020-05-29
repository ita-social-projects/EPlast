using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public class ApproverRepository : RepositoryBase<Approver>, IApproverRepository
    {
        public ApproverRepository(EPlastDBContext dbContext)
           : base(dbContext)
        {
        }
    }
}
