using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Strategy.Core.Domain.Base;
using Strategy.Core.Domain.Interfaces.Repositories;
using Strategy.Core.Domain.Settings;

namespace Strategy.Core.Infra.Repositories
{
    public class MongoGenericService<TDocument> : IMongoGenericService<TDocument> where TDocument : ICollectionBase
    {
        private readonly IMongoCollection<TDocument> _collection;
        private readonly MongoSettings _settings;

        public MongoGenericService(MongoSettings settings)
        {
            _settings = settings;

            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
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

        public void Create(TDocument model)
        {
            _collection.InsertOne(model);
        }

        public void Update(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update)
        {
            _collection.UpdateOne(filter, update);
        }

        public TDocument Get(FilterDefinition<TDocument> filter)
        {
            return _collection.Find(filter).FirstOrDefault();
        }

        public void Remove(FilterDefinition<TDocument> filter)
        {
            _collection.DeleteOne(filter);
        }
    }
}