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
        private readonly IMapper _mapper;

        public UpdateHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            Decesion decision = await _repoWrapper.Decesion.GetFirstAsync(x => x.ID == request.decisionDTO.ID);
            decision.Name = request.decisionDTO.Name;
            decision.Description = request.decisionDTO.Description;
            decision.DecesionStatusType = (DecesionStatusType)request.decisionDTO.DecisionStatusType;
            _repoWrapper.Decesion.Update(decision);
            await _repoWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
