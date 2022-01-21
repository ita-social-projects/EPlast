using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Queries.City;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class GetCityLogoBase64Handler : IRequestHandler<GetCityLogoBase64Query, string>
    {
        private readonly ICityBlobStorageRepository _cityBlobStorage;

        public GetCityLogoBase64Handler(ICityBlobStorageRepository cityBlobStorage)
        {
            _cityBlobStorage= cityBlobStorage;
        }

        public async Task<string> Handle(GetCityLogoBase64Query request, CancellationToken cancellationToken)
        {
            var logoBase64 = await _cityBlobStorage.GetBlobBase64Async(request.LogoName);
            
            return logoBase64;
        }
    }
}
