namespace RushHour.Data.Implementations
{
    using Contracts;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly DbSet<TEntity> dbSet;
        protected readonly RushHourDbContext context;

        public Repository(RushHourDbContext context)
        {
            dbSet = context.Set<TEntity>();
            this.context = context;
        }

        public async Task Delete(TEntity item)
        {
            dbSet.Remove(item);
            context.Entry(item).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool> where = null)
        {
            if (where == null)
                return dbSet;
            else
                return dbSet.Where(where);
        }

        public async Task<TEntity> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task Insert(TEntity item)
        {
            await dbSet.AddAsync(item);
            await context.SaveChangesAsync();
        }

        public async Task Update(TEntity item)
        {
            dbSet.Update(item);
            await context.SaveChangesAsync();
        }
    }
}
