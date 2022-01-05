using EPlast.BLL.DTO.Club;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Queries.Club
{
    public class GetAllClubsByPageAndIsArchiveQuery : IRequest<Tuple<IEnumerable<ClubObjectDTO>, int>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string ClubName { get; set; }
        public bool IsArchived { get; set; }
        public GetAllClubsByPageAndIsArchiveQuery(int page, int pageSize, string clubName, bool isArchived)
        {
            Page = page;
            PageSize = pageSize;
            ClubName = clubName;
            IsArchived = isArchived;
        }
    }
}
