using System.Collections.Generic;
using EPlast.DataAccess.Entities.EducatorsStaff;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.Repositories
{
    public class EducatorsStaffRepository: RepositoryBase<EducatorsStaff>, IEducatorsStaffRepository
    {
        public EducatorsStaffRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }

    }
}
