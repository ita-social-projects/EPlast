using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Interfaces.RegionAdministrations;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;

namespace EPlast.BLL.Interfaces.UserAccess
{
    public interface IUserAccessWrapper
    {
        IClubAccessService ClubAccessService { get; }
        ICityAccessService CityAccessService { get; }
        IRegionAccessService RegionAccessService { get; }
        IAnnualReportAccessService AnnualReportAccessService { get; }
        IUserProfileAccessService UserProfileAccessService { get; }
        IEventUserAccessService EventAccessService { get; }
        IRegionAdministrationAccessService RegionAdministrationAccessService { get; }
    }
}
