using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public class CommentRepository: RepositoryBase<Entities.Comment>, ICommentRepository
    {
        public CommentRepository(EPlastDBContext dbContext)
           : base(dbContext)
        {
        }
    }
}
