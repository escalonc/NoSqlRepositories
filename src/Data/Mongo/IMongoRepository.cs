using System.Linq;
using Core.Contracts;
using Core.Models;

namespace Data.Mongo
{
    public interface IMongoRepository<T> : INoSqlRepository<T> where T : BaseEntity
    {
        IQueryable Filter(bool onlyEnabledEntities);
    }
}