using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.City.CityAccess.CityAccessGetters
{
    public class CItyAccessForRegionAdminGetter : ICItyAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CItyAccessForRegionAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IEnumerable<DatabaseEntities.City>> GetCities(string userId)
        {
            var regionAdministration = await _repositoryWrapper.RegionAdministration.GetFirstOrDefaultAsync(
                    predicate: r => r.User.Id == userId && r.EndDate == null,
                    include: source => source
                        .Include(r => r.Region));
            return regionAdministration != null ? await _repositoryWrapper.City.GetAllAsync(predicate: c => c.Region.ID == regionAdministration.Region.ID)
                : Enumerable.Empty<DatabaseEntities.City>();
        }
    }
}