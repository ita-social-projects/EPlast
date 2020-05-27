using EPlast.BussinessLayer.ExtensionMethods;
using EPlast.Models.Enums;
using EPlast.ViewModels.City;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.ViewModels
{
    public class CreateEditAnnualReportViewModel
    {
        public string CityName { get; set; }
        public IEnumerable<SelectListItem> CityMembers { get; set; }
        public IEnumerable<SelectListItem> CityLegalStatusTypes { get; set; }
        public AnnualReportViewModel AnnualReport { get; set; }

        public CreateEditAnnualReportViewModel(IEnumerable<CityMembersViewModel> cityMembers)
        {
            this.InitializeCityMembers(cityMembers);
            this.InitializeCityLegalStatusTypes();
        }

        private void InitializeCityMembers(IEnumerable<CityMembersViewModel> cityMembers)
        {
            CityMembers = new List<SelectListItem>();
            foreach (var cityMember in cityMembers)
            {
                CityMembers = CityMembers.Append(new SelectListItem
                {
                    Value = cityMember.UserId,
                    Text = $"{cityMember.User.FirstName} {cityMember.User.LastName}"
                });
            }
        }

        private void InitializeCityLegalStatusTypes()
        {
            CityLegalStatusTypes = new List<SelectListItem>();
            foreach (Enum item in Enum.GetValues(typeof(CityLegalStatusType)))
            {
                CityLegalStatusTypes = CityLegalStatusTypes.Append(new SelectListItem
                {
                    Value = item.ToString(),
                    Text = item.GetDescription()
                });
            }
        }
    }
}