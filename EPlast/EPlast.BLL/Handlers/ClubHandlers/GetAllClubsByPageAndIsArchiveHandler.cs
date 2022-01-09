using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.ClubHandlers
{
    public class GetAllClubsByPageAndIsArchiveHandler : IRequestHandler<GetAllClubsByPageAndIsArchiveQuery, Tuple<IEnumerable<ClubObjectDTO>, int>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetAllClubsByPageAndIsArchiveHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Tuple<IEnumerable<ClubObjectDTO>, int>> Handle(GetAllClubsByPageAndIsArchiveQuery request, CancellationToken cancellationToken)
        {
            var filter = GetFilter(request.ClubName, request.IsArchived);
            var order = GetOrder();
            var selector = GetSelector();
            var tuple = await _repositoryWrapper.Club.GetRangeAsync(filter, selector, order,null, request.Page, request.PageSize);
            var clubs = tuple.Item1;
            var rows = tuple.Item2;

            return new Tuple<IEnumerable<ClubObjectDTO>, int>(_mapper.Map<IEnumerable<Club>, IEnumerable<ClubObjectDTO>>(clubs), rows);
        }

        private Expression<Func<Club, bool>> GetFilter(string clubName, bool isArchive)
        {
            var clubNameEmty = string.IsNullOrEmpty(clubName);
            Expression<Func<Club, bool>> expr = (clubNameEmty) switch
            {
                true => x => x.IsActive == isArchive,
                false => x => x.Name.Contains(clubName) && x.IsActive == isArchive
            };
            return expr;
        }

        private Func<IQueryable<Club>, IQueryable<Club>> GetOrder()
        {
            Func<IQueryable<Club>, IQueryable<Club>> expr = x => x.OrderBy(e => e.ID);
            return expr;
        }

        private Expression<Func<Club, Club>> GetSelector()
        {
            Expression<Func<Club, Club>> expr = x => new Club { ID = x.ID, Logo = x.Logo, Name = x.Name };
            return expr;
        }
    }
}
