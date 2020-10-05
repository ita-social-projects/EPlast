using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories.Interfaces.Blank;

namespace EPlast.DataAccess.Repositories.Realizations.Blank
{
    class BlankBiographyDocumentsTypeRepository : RepositoryBase<BlankBiographyDocumentsType>, IBlankBiographyDocumentsTypeRepository
    {
        public BlankBiographyDocumentsTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
    {
    }
}
}
