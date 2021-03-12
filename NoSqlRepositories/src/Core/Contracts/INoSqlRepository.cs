using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Contracts
{
    public interface INoSqlRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> FindAll(bool onlyEnabledEntities);

        Task<T> FindById(Guid id, bool onlyEnabledEntities);

        Task Add(T entity);

        Task Update(T entity);

        Task Delete(T entity);
    }
}