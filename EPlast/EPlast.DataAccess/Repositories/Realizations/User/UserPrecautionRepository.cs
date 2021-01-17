using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.DataAccess.Repositories
{
    public class UserPrecautionRepository : RepositoryBase<UserPrecaution>, IUserPrecautionRepository
    {
        public UserPrecautionRepository(EPlastDBContext dBContext)
            : base(dBContext)
        {

        }
    }
}
