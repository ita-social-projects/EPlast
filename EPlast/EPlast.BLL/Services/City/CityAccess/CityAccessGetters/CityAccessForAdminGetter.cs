using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.City.CityAccess.CityAccessGetters
{
    public class CityAccessForAdminGetter : ICItyAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CityAccessForAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IEnumerable<DatabaseEntities.City>> GetCities(string userId)
        {
            var cityRange = await _repositoryWrapper.City.GetRangeAsync(
                null, null, с => с.OrderBy(x => x.Name), source => source.Include(c => c.Region), null, null);

            return cityRange.Item1;
        }

        public async Task<IEnumerable<Tuple<int, string>>> GetCitiesIdAndName(string userId)
        {
            return (await _repositoryWrapper.City.GetAllAsync())
                .Select(c => new Tuple<int, string>(c.ID, c.Name))
                .ToList();
        }
    }
}