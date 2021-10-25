using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Club;


namespace EPlast.DataAccess.Repositories.Realizations.Club
{
    class ClubReportCitiesRepository : RepositoryBase<ClubReportCities>, IClubReportCitiesRepository
    {
        public ClubReportCitiesRepository(EPlastDBContext dbContext):base(dbContext)
        {
        }
    }
}

