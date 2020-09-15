using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.DataAccess.Repositories.Realizations.EducatorsStaff
{
    public class EducatorsSatffTypesRepository: RepositoryBase<EducatorsStaffTypes>, IEducatorsStaffTypesRepository
    {
        public EducatorsSatffTypesRepository(EPlastDBContext dbContext)
       : base(dbContext)
        {
        }

    }
}
