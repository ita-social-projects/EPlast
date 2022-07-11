using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Club;
using MediatR;

namespace EPlast.BLL.Queries.Club
{
    public class GetClubDataForReportQuery : IRequest<ClubReportDataDto>
    {
        public int ClubId { get; set; }
        public GetClubDataForReportQuery(int clubId)
        {
            ClubId = clubId;
        }
    }
}
