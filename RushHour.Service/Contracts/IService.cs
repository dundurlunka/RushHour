namespace RushHour.Service.Contracts
{
    using Data.Contracts;
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IService<TEntity> where TEntity : class, IEntity
    {
        Task InsertAsync(TEntity item);

        Task UpdateAsync(TEntity item);

        Task Delete(TEntity item);

        Task<TEntity> GetByIdAsync(int id);

        IEnumerable<TEntity> GetAll(Func<TEntity, bool> where = null);

        Task<bool> IsUserAuthorized(TEntity item, User currentUser);

        Task<IEnumerable<TEntity>> GetFilteredItemsAsync(User currentUser);

        bool IsItemDuplicate(TEntity item);
    }
}
