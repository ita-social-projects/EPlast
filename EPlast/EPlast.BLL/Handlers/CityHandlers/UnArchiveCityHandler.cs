using EPlast.BLL.Commands.City;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using EPlast.DataAccess.Repositories;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class UnArchiveCityHandler : IRequestHandler<UnArchiveCityCommand>
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public UnArchiveCityHandler(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<Unit> Handle(UnArchiveCityCommand request, CancellationToken cancellationToken)
        {
            var city = await _repoWrapper.City.GetFirstOrDefaultAsync(c => c.ID == request.CityId && !c.IsActive);
            city.IsActive = true;
            _repoWrapper.City.Update(city);
            await _repoWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
