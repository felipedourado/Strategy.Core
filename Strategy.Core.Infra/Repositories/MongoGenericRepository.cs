using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Strategy.Core.Domain.Base;
using Strategy.Core.Domain.Interfaces.Repositories;
using Strategy.Core.Domain.Settings;
using System.Linq.Expressions;
using System.Reflection;

namespace Strategy.Core.Infra.Repositories
{
    public class MongoGenericRepository<TDocument> : IMongoGenericRepository<TDocument> where TDocument : IMongoDocumentBase
    {
        private readonly IMongoCollection<TDocument> _collection;
        private readonly MongoSettings _settings;

        public MongoGenericRepository(MongoSettings settings)
        {
            _settings = settings;

            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);
            var database = new MongoClient(_settings.ConnectionString).GetDatabase(_settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected string? GetCollectionName(Type documentType)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return ((MongoCustomAttribute)documentType.GetCustomAttributes(
                    typeof(MongoCustomAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        #region Create

        public void Create(TDocument entity)
        {
            entity = UpdatedObject(entity);
            _collection.InsertOne(entity);
        }

        public async Task CreateAsync(TDocument entity)
        {
            entity = UpdatedObject(entity);
            await _collection.InsertOneAsync(entity);
        }

        public void InsertMany(IEnumerable<TDocument> entities)
        {
            entities = UpdatedManyObject(entities);
            _collection.InsertMany(entities);
        }

        public async Task InsertManyAsync(IEnumerable<TDocument> entities)
        {
            entities = UpdatedManyObject(entities);
            await _collection.InsertManyAsync(entities);
        }

        #endregion

        #region Update

        public void Update(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update)
        {
            _collection.UpdateOne(filter, update);
        }

        public void ReplaceOne(TDocument document)
        {
            document = this.UpdatedObject(document);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace<TDocument>(filter, document);
        }

        public async Task ReplaceOneAsync(TDocument document)
        {
            document = this.UpdatedObject(document);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);

            await _collection.FindOneAndReplaceAsync<TDocument>(filter, document);
        }

        public ReplaceOneResult ReplaceOrInsert(Expression<Func<TDocument, bool>> filter, TDocument document)
        {
            document = this.UpdatedObjectForReplace(document);
            var result = _collection.ReplaceOne<TDocument>(filter, document, new ReplaceOptions { IsUpsert = true });

            return result;
        }

        public async Task<ReplaceOneResult> ReplaceOrInsertAsync(Expression<Func<TDocument, bool>> filter, TDocument document)
        {
            document = this.UpdatedObjectForReplace(document);
            var result = await _collection.ReplaceOneAsync<TDocument>(filter, document, new ReplaceOptions { IsUpsert = true });

            return result;
        }

        public UpdateResult UpdateOneBuilder(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> document)
        {
            var date = DateTime.UtcNow;
            var dateNow = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, date.Kind);
            var result = _collection.UpdateOne(filterExpression, document);
            var updateDef = Builders<TDocument>.Update.Set(item => item.UpdateDate, dateNow);
            _ = _collection.UpdateOne(filterExpression, updateDef);

            return result;
        }

        public async Task<UpdateResult> UpdateOneBuilderAsync(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> document)
        {
            var date = DateTime.UtcNow;
            var dateNow = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, date.Kind);
            var result = await _collection.UpdateOneAsync(filterExpression, document);
            var updateDef = Builders<TDocument>.Update.Set(item => item.UpdateDate, dateNow);
            await _collection.UpdateOneAsync(filterExpression, updateDef);

            return result;
        }

        public async Task<UpdateResult> UpdateOneBuilderAsync(FilterDefinition<TDocument> filterDefinition, UpdateDefinition<TDocument> document)
        {
            var date = DateTime.UtcNow;
            var dateNow = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, date.Kind);
            var updateDef = document.Set(x => x.UpdateDate, dateNow);
            var result = await _collection.UpdateOneAsync(filterDefinition, updateDef);

            return result;
        }

        public long UpdateMany(IEnumerable<TDocument> documents)
        {
            documents = UpdatedManyObject(documents);

            var updates = new List<WriteModel<TDocument>>();
            var filterBuilder = Builders<TDocument>.Filter;

            foreach (var doc in documents)
            {
                foreach (PropertyInfo prop in typeof(TDocument).GetProperties())
                {
                    if (prop.Name == "Id")
                    {
                        var filter = filterBuilder.Eq(prop.Name, prop.GetValue(doc));
                        updates.Add(new ReplaceOneModel<TDocument>(filter, doc));
                        break;
                    }
                }
            }
            BulkWriteResult result = _collection.BulkWrite(updates);

            return result.ModifiedCount;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<TDocument> documents)
        {
            documents = UpdatedManyObject(documents);

            var updates = new List<WriteModel<TDocument>>();
            var filterBuilder = Builders<TDocument>.Filter;

            foreach (var doc in documents)
            {
                foreach (PropertyInfo prop in typeof(TDocument).GetProperties())
                {
                    if (prop.Name == "Id")
                    {
                        var filter = filterBuilder.Eq(prop.Name, prop.GetValue(doc));
                        updates.Add(new ReplaceOneModel<TDocument>(filter, doc));
                        break;
                    }
                }
            }
            BulkWriteResult result = await _collection.BulkWriteAsync(updates);

            return result.ModifiedCount;
        }

        public long UpdateMany(IEnumerable<TDocument> documents, UpdateDefinition<TDocument> update)
        {
            documents = UpdatedManyObject(documents);

            var updates = new List<WriteModel<TDocument>>();
            var filterBuilder = Builders<TDocument>.Filter;

            foreach (var doc in documents)
            {
                foreach (PropertyInfo prop in typeof(TDocument).GetProperties())
                {
                    if (prop.Name == "Id")
                    {
                        var filter = filterBuilder.Eq(prop.Name, prop.GetValue(doc));
                        updates.Add(new UpdateOneModel<TDocument>(filter, update));
                        break;
                    }
                }
            }
            BulkWriteResult result = _collection.BulkWrite(updates);

            return result.ModifiedCount;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<TDocument> documents, UpdateDefinition<TDocument> update)
        {
            documents = UpdatedManyObject(documents);

            var updates = new List<WriteModel<TDocument>>();
            var filterBuilder = Builders<TDocument>.Filter;

            foreach (var doc in documents)
            {
                foreach (PropertyInfo prop in typeof(TDocument).GetProperties())
                {
                    if (prop.Name == "Id")
                    {
                        var filter = filterBuilder.Eq(prop.Name, prop.GetValue(doc));
                        updates.Add(new UpdateOneModel<TDocument>(filter, update));
                        break;
                    }
                }
            }
            BulkWriteResult result = await _collection.BulkWriteAsync(updates);

            return result.ModifiedCount;
        }

        private TDocument UpdatedObject(TDocument document)
        {
            var date = DateTime.UtcNow;
            var dateNow = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, date.Kind);

            if (document is IMongoDocumentBase iDocument) iDocument.UpdateDate = dateNow;

            return document;
        }

        private TDocument UpdatedObjectForReplace(TDocument document)
        {
            document.Id = ObjectId.GenerateNewId();
            var date = DateTime.UtcNow;
            var dateNow = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, date.Kind);

            if (document is IMongoDocumentBase iDocument) iDocument.UpdateDate = dateNow;

            return document;
        }

        private IEnumerable<TDocument> UpdatedManyObject(IEnumerable<TDocument> collection)
        {
            var date = DateTime.UtcNow;
            var dateNow = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, date.Kind);

            if (collection.FirstOrDefault() is IMongoDocumentBase iDocument)
            {
                foreach (var document in collection)
                {
                    document.UpdateDate = dateNow;
                }
            }

            return collection;
        }

        #endregion

        #region Delete

        public void Remove(FilterDefinition<TDocument> filter)
        {
            _collection.DeleteOne(filter);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await _collection.FindOneAndDeleteAsync(filterExpression);
        }

        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            _collection.FindOneAndDelete(filter);
        }

        public async Task DeleteByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            await _collection.FindOneAndDeleteAsync(filter);
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await _collection.DeleteManyAsync(filterExpression);
        }

        public void DeleteMany(FilterDefinition<TDocument> filter)
        {
            _collection.DeleteMany(filter);
        }

        public async Task DeleteManyAsync(FilterDefinition<TDocument> filter)
        {
            await _collection.DeleteManyAsync(filter);
        }

        #endregion

        #region Get & Find

        public IMongoCollection<TDocument> GetCollection()
        {
            return _collection;
        }

        public TDocument Get(FilterDefinition<TDocument> filter)
        {
            return _collection.Find(filter).FirstOrDefault();
        }

        public IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public async Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression).ToListAsync();
        }

        public async Task<IEnumerable<TDocument>> FilterByAsync(FilterDefinition<TDocument> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }

        public IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public async Task<IEnumerable<TProjected>> FilterByAsync<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return await _collection.Find(filterExpression).Project(projectionExpression).ToListAsync();
        }

        public TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression).FirstOrDefaultAsync();
        }

        public TDocument FindLastOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).SortByDescending(item => item.CreateDate).FirstOrDefault();
        }

        public TDocument FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            return _collection.Find(filter).SingleOrDefault();
        }

        public async Task<TDocument> FindByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        #endregion 
    }
}