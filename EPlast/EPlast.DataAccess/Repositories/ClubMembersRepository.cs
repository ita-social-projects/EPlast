using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public class ClubMembersRepository : RepositoryBase<ClubMembers>, IClubMembersRepository
    {
        public ClubMembersRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
