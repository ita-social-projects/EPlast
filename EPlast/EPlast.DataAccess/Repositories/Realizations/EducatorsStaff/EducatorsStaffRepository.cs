using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.DataAccess.Repositories
{
    public class EducatorsStaffRepository: RepositoryBase<EducatorsStaff>, IEducatorsStaffRepository
    {
        public EducatorsStaffRepository(EPlastDBContext dbContext)
       : base(dbContext)
        {
        }
    }
}
