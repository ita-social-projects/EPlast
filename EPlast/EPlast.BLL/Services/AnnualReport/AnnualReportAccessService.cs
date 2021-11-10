using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace EPlast.BLL.Services
{
    public class AnnualReportAccessService : IAnnualReportAccessService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AnnualReportAccessService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }


        public async Task<bool> CanEditCityReportAsync(string userId, int reportId)
        {
            var annualReport =
                _repositoryWrapper.AnnualReports.FindByCondition(x => (x.CreatorId == userId && x.ID == reportId));
            return annualReport.Any();
        }
    }
}