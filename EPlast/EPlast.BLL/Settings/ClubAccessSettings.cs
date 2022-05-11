using EPlast.BLL.Services.Club.ClubAccess.ClubAccessGetters;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using System.Collections.Generic;

namespace EPlast.BLL.Settings
{
    public class ClubAccessSettings
    {
        private const string AdminRoleName = Roles.Admin;
        private const string GoverningBodyAdminRoleName = Roles.GoverningBodyAdmin;
        private const string ClubAdminRoleName = Roles.KurinHead;
        private const string ClubAdminDeputyRoleName = Roles.KurinHeadDeputy;

        private readonly IRepositoryWrapper _repositoryWrapper;

        public ClubAccessSettings(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Dictionary<string, IClubAccessGetter> ClubAccessGetters
        {
            get
            {
                return new Dictionary<string, IClubAccessGetter>
                {
                    { AdminRoleName,  new ClubAccessForAdminGetter(_repositoryWrapper) },
                    { GoverningBodyAdminRoleName,  new ClubAccessForAdminGetter(_repositoryWrapper) },
                    { ClubAdminRoleName, new ClubAccessForClubAdminGetter(_repositoryWrapper) },
                    { ClubAdminDeputyRoleName, new ClubAccessForClubAdminGetter(_repositoryWrapper) }
                };
            }
        }

    }
}
