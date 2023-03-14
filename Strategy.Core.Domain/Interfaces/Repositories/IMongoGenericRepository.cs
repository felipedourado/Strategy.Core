using MongoDB.Driver;
using Strategy.Core.Domain.Base;
using System.Linq.Expressions;

namespace Strategy.Core.Domain.Interfaces.Repositories
{
    public interface IMongoGenericRepository<TDocument> where TDocument : IMongoDocumentBase
    {
        #region Create
        void Create(TDocument request);
        Task CreateAsync(TDocument entity);
        void InsertMany(IEnumerable<TDocument> entities);
        Task InsertManyAsync(IEnumerable<TDocument> entities);
        #endregion

        #region Update
        void Update(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update);
        void ReplaceOne(TDocument document);
        Task ReplaceOneAsync(TDocument document);
        ReplaceOneResult ReplaceOrInsert(Expression<Func<TDocument, bool>> filter, TDocument document);
        Task<ReplaceOneResult> ReplaceOrInsertAsync(Expression<Func<TDocument, bool>> filter, TDocument document);
        UpdateResult UpdateOneBuilder(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> document);
        Task<UpdateResult> UpdateOneBuilderAsync(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> document);
        Task<UpdateResult> UpdateOneBuilderAsync(FilterDefinition<TDocument> filterDefinition, UpdateDefinition<TDocument> document);
        long UpdateMany(IEnumerable<TDocument> documents);
        Task<long> UpdateManyAsync(IEnumerable<TDocument> documents);
        long UpdateMany(IEnumerable<TDocument> documents, UpdateDefinition<TDocument> update);
        Task<long> UpdateManyAsync(IEnumerable<TDocument> documents, UpdateDefinition<TDocument> update);
        #endregion

        #region Delete
        void Remove(FilterDefinition<TDocument> filter);
        void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        void DeleteById(string id);
        Task DeleteByIdAsync(string id);
        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
        void DeleteMany(FilterDefinition<TDocument> filter);
        Task DeleteManyAsync(FilterDefinition<TDocument> filter);
        #endregion

        #region Get & Find
        IMongoCollection<TDocument> GetCollection();
        TDocument Get(FilterDefinition<TDocument> filter);
        IQueryable<TDocument> AsQueryable();
        IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression);
        Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task<IEnumerable<TDocument>> FilterByAsync(FilterDefinition<TDocument> filter);
        IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);
        Task<IEnumerable<TProjected>> FilterByAsync<TProjected>(Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);
        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);
        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        TDocument FindLastOne(Expression<Func<TDocument, bool>> filterExpression);
        TDocument FindById(string id);
        Task<TDocument> FindByIdAsync(string id);
        #endregion

    }
}