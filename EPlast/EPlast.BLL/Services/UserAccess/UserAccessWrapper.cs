﻿using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Interfaces.RegionAdministrations;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;

namespace EPlast.BLL.Services.UserAccess
{
    public class UserAccessWrapper : IUserAccessWrapper
    {
        public IClubAccessService ClubAccessService { get => _clubAccessService; }
        public ICityAccessService CityAccessService { get => _cityAccessService; }
        public IRegionAccessService RegionAccessService { get => _regionAccessService; }
        public IAnnualReportAccessService AnnualReportAccessService { get => _annualReportAccessService; }
        public IUserProfileAccessService UserProfileAccessService { get => _userProfileAccessService; }
        public IEventUserAccessService EventAccessService { get => _eventAccessService; }
        public IRegionAdministrationAccessService RegionAdministrationAccessService { get => _regionAdministrationAccessService; }

        public UserAccessWrapper(
            IClubAccessService clubAccessService,
            ICityAccessService cityAccessService,
            IRegionAccessService regionAccessService,
            IAnnualReportAccessService annualReportAccessService,
            IUserProfileAccessService userProfileAccessService,
            IEventUserAccessService eventAccessService,
            IRegionAdministrationAccessService regionAdministrationAccessService)
        {
            _cityAccessService = cityAccessService;
            _clubAccessService = clubAccessService;
            _regionAccessService = regionAccessService;
            _annualReportAccessService = annualReportAccessService;
            _userProfileAccessService = userProfileAccessService;
            _eventAccessService = eventAccessService;
            _regionAdministrationAccessService = regionAdministrationAccessService;
        }

        private readonly IClubAccessService _clubAccessService;
        private readonly ICityAccessService _cityAccessService;
        private readonly IRegionAccessService _regionAccessService;
        private readonly IAnnualReportAccessService _annualReportAccessService;
        private readonly IUserProfileAccessService _userProfileAccessService;
        private readonly IEventUserAccessService _eventAccessService;
        private readonly IRegionAdministrationAccessService _regionAdministrationAccessService;
    }
}
