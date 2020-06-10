using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<T>> FindAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = this.GetQueryable(includeProperties);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets a list of entities.
        /// </summary>
        /// <param name="includeProperties">A list of submodels that should be included in the search result.</param>
        /// <returns>A list of all entities (with deffered execution via <see cref="IQueryable">)</returns>
        private IQueryable<T> GetQueryable(params Expression<Func<T, object>>[] includeProperties) // We can add some filters, static filter, ordering and etc
        {
            IQueryable<T> queryable = this.EPlastDBContext.Set<T>().AsNoTracking(); // ?
            if (includeProperties != null)
            {
                queryable = includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
            }

            return queryable;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.EPlastDBContext.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> query = this.BuildQuery(expression);

            return await query.ToListAsync();
        }

        private IQueryable<T> BuildQuery(Expression<Func<T, bool>> predicate) //TODO: add includeProperties, ordering, count or etc
        {
            IQueryable<T> query = this.EPlastDBContext.Set<T>().AsNoTracking(); //?
            if(predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
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