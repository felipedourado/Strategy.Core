using System.Security.AccessControl;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Strategy.Core.Repositories.Interfaces;

namespace Strategy.Core.Repositories
{
    public class MongoGenericService<T> : IMongoGenericService<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;
        private readonly IConfiguration _configuration;

        public MongoGenericService(IConfiguration configuration)
        {
            _configuration = configuration;
            // var settings = AppSettings.Get<T>("MongoConnection");
            var client = new MongoClient(_configuration["ConnectionString"]);
            var database = client.GetDatabase(_configuration["DatabaseName"]);
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public void Create(T model)
        {
            _collection.InsertOne(model);
        }

        public void Update(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            _collection.UpdateOne(filter, update);
        }

        public T Get(FilterDefinition<T> filter)
        {
            return _collection.Find(filter).FirstOrDefault();
        }

        public void Remove(FilterDefinition<T> filter)
        {
            _collection.DeleteOne(filter);
        }
    }
}