using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Commands.City;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class EditCityHandler : IRequestHandler<EditCityCommand>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryWrapper _repoWrapper;

        public EditCityHandler(IMediator mediator, IRepositoryWrapper repoWrapper)
        {
            _mediator = mediator;
            _repoWrapper = repoWrapper;
        }

        public async Task<Unit> Handle(EditCityCommand request, CancellationToken cancellationToken)
        {
            var uploadPhotoCommand = new UploadCityPhotoCommand(request.City);
            await _mediator.Send(uploadPhotoCommand, cancellationToken);

            var createCityCommand = new CreateCityCommand(request.City);
            var city = await _mediator.Send(createCityCommand, cancellationToken);

            _repoWrapper.City.Attach(city);
            _repoWrapper.City.Update(city);
            await _repoWrapper.SaveAsync();
            
            return Unit.Value;
        }
    }
}
