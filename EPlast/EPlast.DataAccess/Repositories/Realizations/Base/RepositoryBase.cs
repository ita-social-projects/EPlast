using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected EPlastDBContext EPlastDBContext { get; set; }

        public RepositoryBase(EPlastDBContext ePlastDBContext)
        {
            this.EPlastDBContext = ePlastDBContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.EPlastDBContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.EPlastDBContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            this.EPlastDBContext.Set<T>().Add(entity);
        }

        public async Task CreateAsync(T entity)
        {
            await this.EPlastDBContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            this.EPlastDBContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.EPlastDBContext.Set<T>().Remove(entity);
        }

        public void Attach(T entity)
        {
            this.EPlastDBContext.Set<T>().Attach(entity);
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IIncludableQueryable<T, object> query = null;

            if (includes.Length > 0)
            {
                query = this.EPlastDBContext.Set<T>().Include(includes[0]);
            }
            for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
            {
                query = query.Include(includes[queryIndex]);
            }

            return query == null ? this.EPlastDBContext.Set<T>() : (IQueryable<T>)query;
        }
    }
}