using EPlast.BLL.DTO.Club;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Queries.Club
{
    public class GetClubDataForReportQuery : IRequest<ClubReportDataDTO>
    {
        public int ClubId { get; set; }
        public GetClubDataForReportQuery(int clubId)
        {
            ClubId = clubId;
        }
    }
}
