using MongoDB.Driver;
using Strategy.Core.Domain.Base;

namespace Strategy.Core.Domain.Interfaces.Repositories
{
    public interface IMongoGenericService<TDocument> where TDocument : IMongoDocumentBase
    {
        void Create(TDocument request);
        void Update(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update);
        TDocument Get(FilterDefinition<TDocument> filter);
        void Remove(FilterDefinition<TDocument> filter);
    }
}