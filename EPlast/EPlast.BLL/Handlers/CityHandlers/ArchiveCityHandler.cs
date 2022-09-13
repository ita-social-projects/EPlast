using System;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Commands.City;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class ArchiveCityHandler : IRequestHandler<ArchiveCityCommand>
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public ArchiveCityHandler(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<Unit> Handle(ArchiveCityCommand request, CancellationToken cancellationToken)
        {
            var city = await _repoWrapper.City.GetFirstOrDefaultAsync(c => c.ID == request.CityId && c.IsActive);
            if (city.CityMembers is null && city.CityAdministration is null)
            {
                city.IsActive = false;
                _repoWrapper.City.Update(city);
                await _repoWrapper.SaveAsync();
            }
            else
            {
                throw new InvalidOperationException("Cannot archive city with members");
            }

            return Unit.Value;
        }
    }
}
