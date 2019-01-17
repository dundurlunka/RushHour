namespace RushHour.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task Insert(TEntity item);

        Task Update(TEntity item);

        Task Delete(TEntity item);

        Task<TEntity> GetById(int id);

        IEnumerable<TEntity> GetAll(Func<TEntity, bool> where = null);
    }
}
