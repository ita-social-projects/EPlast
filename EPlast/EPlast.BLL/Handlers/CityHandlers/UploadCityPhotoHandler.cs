using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Commands.City;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class UploadCityPhotoHandler : IRequestHandler<UploadCityPhotoCommand>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ICityBlobStorageRepository _cityBlobStorage;
        private readonly IUniqueIdService _uniqueId;

        public UploadCityPhotoHandler(IRepositoryWrapper repoWrapper, 
            ICityBlobStorageRepository cityBlobStorage,
            IUniqueIdService uniqueId)
        {
            _repoWrapper = repoWrapper;
            _cityBlobStorage = cityBlobStorage;
            _uniqueId = uniqueId;
        }

        public async Task<Unit> Handle(UploadCityPhotoCommand request, CancellationToken cancellationToken)
        {
            var oldImageName = (await _repoWrapper.City.GetFirstOrDefaultAsync(i => i.ID == request.City.ID))?.Logo;
            var logoBase64 = request.City.Logo;

            if (!string.IsNullOrWhiteSpace(logoBase64) && logoBase64.Length > 0)
            {
                var logoBase64Parts = logoBase64.Split(',');
                var extension = logoBase64Parts[0].Split(new[] { '/', ';' }, 3)[1];

                if (!string.IsNullOrEmpty(extension))
                {
                    extension = (extension[0] == '.' ? "" : ".") + extension;
                }

                var fileName = $"{_uniqueId.GetUniqueId()}{extension}";

                await _cityBlobStorage.UploadBlobForBase64Async(logoBase64Parts[1], fileName);
                request.City.Logo = fileName;
            }

            if (!string.IsNullOrEmpty(oldImageName))
            {
                await _cityBlobStorage.DeleteBlobAsync(oldImageName);
            }

            return Unit.Value;
        }
    }
}
