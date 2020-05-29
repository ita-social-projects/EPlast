using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        void Create(T entity);
        Task CreateAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Attach(T entity);

        IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
    }
}