using EPlast.DataAccess.Repositories;
using MediatR;
using EPlast.BLL.Commands.Decision;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class UpdateHandler: IRequestHandler<UpdateCommand>
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public UpdateHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        public async Task<Unit> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            Decesion decision = await _repoWrapper.Decesion.GetFirstAsync(x => x.ID == request.DecisionDto.ID);
            decision.Name = request.DecisionDto.Name;
            decision.Description = request.DecisionDto.Description;
            decision.DecesionStatusType = (DecesionStatusType)request.DecisionDto.DecisionStatusType;
            _repoWrapper.Decesion.Update(decision);
            await _repoWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
