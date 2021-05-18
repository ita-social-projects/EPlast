﻿using EPlast.BLL.Services.Club.ClubAccess.ClubAccessGetters;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using EPlast.Resources;

namespace EPlast.BLL.Settings
{
    public class ClubAccessSettings
    {
        private const string AdminRoleName = Roles.Admin;
        private const string RegionAdminRoleName = Roles.OkrugaHead;
        private const string CityAdminRoleName = Roles.CityHead;
        private const string ClubAdminRoleName = Roles.KurinHead;

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
                    { ClubAdminRoleName, new ClubAccessForClubAdminGetter(_repositoryWrapper) }
                };
            }
        }

    }
}
