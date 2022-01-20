using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;

namespace EPlast.BLL.Handlers.ClubHandlers
{
    public class GetClubDataForReportHandler : IRequestHandler<GetClubDataForReportQuery, ClubReportDataDTO>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetClubDataForReportHandler(IRepositoryWrapper repoWrapper, IMapper mapper, IMediator mediator)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ClubReportDataDTO> Handle(GetClubDataForReportQuery request, CancellationToken cancellationToken)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(
               predicate: c => c.ID == request.ClubId);

            if (club == null)
            {
                return default;
            }

            var clubAdmins = await _mediator.Send(new GetClubAdministrationsQuery(request.ClubId));
            var clubDto = _mapper.Map<Club, ClubDTO>(club);
            var clubHead = clubAdmins.FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.KurinHead);
            var head = _mapper.Map<ClubAdministration, ClubReportAdministrationDTO>(clubHead);
            var clubAdminsDto = _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubReportAdministrationDTO>>(clubAdmins);
            var clubHistoryFollowersDTO = await _mediator.Send(new GetClubHistoryFollowersQuery(request.ClubId));
            var clubHistoryMembersDTO = await _mediator.Send(new GetClubHistoryMembersQuery(request.ClubId));

            var clubProfileDto = new ClubReportDataDTO
            {
                Club = clubDto,
                Head = head,
                Members = clubHistoryMembersDTO.ToList(),
                Followers = clubHistoryFollowersDTO.ToList(),
                Admins = clubAdminsDto.ToList(),
                CountUsersPerYear = await _mediator.Send(new GetCountUsersPerYearQuery(request.ClubId)),
                CountDeletedUsersPerYear = await _mediator.Send(new GetCountDeletedUsersPerYearQuery(request.ClubId)),
                PhoneNumber = club.PhoneNumber,
                Email = club.Email,
                ClubURL = club.ClubURL,
                Slogan = club.Slogan
            };

            return clubProfileDto;
        }
    }
}
