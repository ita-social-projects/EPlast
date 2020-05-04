using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.Models.ViewModelInitializations.Interfaces
{
    public interface IAnnualReportVMInitializer
    {
        IEnumerable<SelectListItem> GetCityMembers(IEnumerable<User> cityMembers);
        IEnumerable<SelectListItem> GetCityLegalStatusTypes();
        AnnualReport GetAnnualReport(string userId, int cityId, IEnumerable<User> cityMember);
    }
}