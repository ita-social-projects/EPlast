using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Commands.City;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class RemoveCityHandler : IRequestHandler<RemoveCityCommand>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ICityBlobStorageRepository _cityBlobStorage;

        public RemoveCityHandler(IRepositoryWrapper repoWrapper, 
            ICityBlobStorageRepository cityBlobStorage)
        {
            _repoWrapper = repoWrapper;
            _cityBlobStorage = cityBlobStorage;
        }

        public async Task<Unit> Handle(RemoveCityCommand request, CancellationToken cancellationToken)
        {
            var city = await _repoWrapper.City.GetFirstOrDefaultAsync(c => c.ID == request.CityId);

            if (city.Logo != null)
            {
                await _cityBlobStorage.DeleteBlobAsync(city.Logo);
            }

            _repoWrapper.City.Delete(city);
            await _repoWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
