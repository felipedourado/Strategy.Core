using MongoDB.Driver;

namespace Strategy.Core.Repositories.Interfaces
{
    public interface IMongoGenericService<T>
    {
        void Create(T model);
        void Update(FilterDefinition<T> filter, UpdateDefinition<T> update);
        T Get(FilterDefinition<T> filter);
        void Remove(FilterDefinition<T> filter);
    }
}