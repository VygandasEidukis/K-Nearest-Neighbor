using K_nearest_neighbors.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace K_nearest_neighbors.Data_Access.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>, IDisposable where TEntity : class
    {
        protected ClassificationContext Context { get; }

        private readonly DbSet<TEntity> dbSet;

        protected GenericRepository(ClassificationContext context)
        {
            this.Context = context;
            this.dbSet = context.Set<TEntity>();
        }

        protected virtual async Task<TEntity> Add(TEntity entity)
        {
            entity = this.dbSet.Add(entity);
            await this.Context.SaveChangesAsync();
            return entity;
        }

        protected virtual IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        protected virtual IQueryable<TEntity> GetQueryables()
        {
            return dbSet.AsQueryable();
        }

        protected virtual async Task<TEntity> Update(TEntity entity)
        {
            var dbEntityEntry = this.Context.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
            await this.Context.SaveChangesAsync();
            return dbEntityEntry.Entity;
        }

        protected virtual async Task<int> SaveChanges()
        {
            return await this.Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
