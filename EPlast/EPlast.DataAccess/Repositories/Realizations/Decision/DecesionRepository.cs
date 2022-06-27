using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Decision;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.Repositories
{
    public class DecesionRepository : RepositoryBase<Decesion>, IDecesionRepository
    {
        public DecesionRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<DecisionTableObject>> GetDecisions(string searchData, int page, int pageSize)
        {
            searchData = searchData?.ToLower();

            var found = EPlastDBContext.Set<Decesion>()
                .Include(d => d.Organization)
                .Include(d=>d.DecesionTarget)
                .Where(d =>
                    string.IsNullOrWhiteSpace(searchData)
                    || ("У розгляді".ToLower().Contains(searchData) && d.DecesionStatusType == DecesionStatusType.Canceled)
                    || ("Підтверджено".ToLower().Contains(searchData) && d.DecesionStatusType == DecesionStatusType.Confirmed)
                    || ("Скасовано".ToLower().Contains(searchData) && d.DecesionStatusType == DecesionStatusType.Canceled)
                    || d.ID.ToString().Contains(searchData)
                    || d.Name.ToLower().Contains(searchData)
                    || d.Organization.OrganizationName.ToLower().Contains(searchData)
                    ||d.DecesionTarget.TargetName.ToLower().Contains(searchData)
                    ||d.Description.ToLower().Contains(searchData)
                    ||d.Date.ToString().Contains(searchData)
                );

            var selected = found
                .Select(d => new DecisionTableObject
                {
                    Id = d.ID,
                    Name = d.Name,
                    Description = d.Description,
                    FileName = d.FileName,
                    UserId = d.UserId,
                    Date = d.Date,
                    GoverningBody = d.Organization.OrganizationName,
                    DecisionTarget = d.DecesionTarget.TargetName,
                    DecisionStatusType = (int)d.DecesionStatusType,
                    Count = found.Count(),
                    Total = EPlastDBContext.Set<Decesion>().Count()
                });

            var items = await selected
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();

            return items;
        }
    }
}
