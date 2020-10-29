using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Club;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Realizations.Club
{
    public class ClubAnnualReportRepository: RepositoryBase<ClubAnnualReport>, IClubAnnualReportsRepository
    {
        public ClubAnnualReportRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {

        }
    }
}
