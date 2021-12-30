using EPlast.BLL.Commands.Club;
using EPlast.DataAccess.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.ClubHandlers
{
    public class ArchiveHandler : IRequestHandler<ArchiveCommand>
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public ArchiveHandler(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<Unit> Handle(ArchiveCommand request, CancellationToken cancellationToken)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(c => c.ID == request.ClubId && c.IsActive);
            if (club.ClubMembers is null && club.ClubAdministration is null)
            {
                club.IsActive = false;
                _repoWrapper.Club.Update(club);
                await _repoWrapper.SaveAsync();
            }
            else
            {
                throw new InvalidOperationException();
            }
            return Unit.Value;
        }
    }
}
