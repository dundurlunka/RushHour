namespace RushHour.Service.Implementations
{
    using Contracts;
    using Data.Contracts;
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class, IEntity
    {
        protected IRepository<TEntity> repository;

        public Service(IRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        public abstract Task<IEnumerable<TEntity>> GetFilteredItemsAsync(User currentUser);

        public abstract bool IsItemDuplicate(TEntity item);

        public abstract Task<bool> IsUserAuthorized(TEntity item, User currentUser);

        public async Task Delete(TEntity item)
        {
            await repository.Delete(item);
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool> where = null)
        {
            return repository.GetAll(where);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await repository.GetById(id);
        }

        public async Task InsertAsync(TEntity item)
        {
            await repository.Insert(item);
        }

        public async Task UpdateAsync(TEntity item)
        {
            await repository.Update(item);
        }
    }
}
