using EPlast.BLL.Commands.Club;
using EPlast.DataAccess.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.ClubHandlers
{
    public class UnArchiveHandler : IRequestHandler<UnArchiveCommand>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        public UnArchiveHandler(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        public async Task<Unit> Handle(UnArchiveCommand request, CancellationToken cancellationToken)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(c => c.ID == request.ClubId && !c.IsActive);
            club.IsActive = true;
            _repoWrapper.Club.Update(club);
            await _repoWrapper.SaveAsync();
            return Unit.Value;
        }
    }
}
