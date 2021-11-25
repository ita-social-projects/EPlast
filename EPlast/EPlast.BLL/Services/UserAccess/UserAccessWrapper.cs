using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;

namespace EPlast.BLL.Services.UserAccess
{
    public class UserAccessWrapper : IUserAccessWrapper
    {
        private IClubAccessService _clubAccessService;
        private ICityAccessService _cityAccessService;
        private IRegionAccessService _regionAccessService;
        private IAnnualReportAccessService _annualReportAccessService;
        private IUserProfileAccessService _userProfileAccessService;
        private IEventUserAccessService _eventAccessService;

        public IClubAccessService ClubAccessService { get => _clubAccessService; }

        public ICityAccessService CityAccessService { get => _cityAccessService; }

        public IRegionAccessService RegionAccessService { get => _regionAccessService; }

        public IAnnualReportAccessService AnnualReportAccessService { get => _annualReportAccessService; }

        public IUserProfileAccessService UserProfileAccessService { get => _userProfileAccessService; }

        public IEventUserAccessService EventAccessService { get => _eventAccessService; }

        public UserAccessWrapper(
            IClubAccessService clubAccessService,
            ICityAccessService cityAccessService,
            IRegionAccessService regionAccessService,
            IAnnualReportAccessService annualReportAccessService,
            IUserProfileAccessService userProfileAccessService,
            IEventUserAccessService eventAccessService
            )
        {
            _cityAccessService = cityAccessService;
            _clubAccessService = clubAccessService;
            _regionAccessService = regionAccessService;
            _annualReportAccessService = annualReportAccessService;
            _userProfileAccessService = userProfileAccessService;
            _eventAccessService = eventAccessService;
        }
    }
}
