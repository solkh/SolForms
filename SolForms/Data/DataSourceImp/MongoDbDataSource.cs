using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SolForms.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Data.DataSourceImp
{
    public class MongoDbDataSource : IFormsDataSource
    {
        private readonly IMongoDatabase _database;
        private readonly MongoDbConnectionOption _options;

        public MongoDbDataSource(IOptions<MongoDbConnectionOption> option)
        {
            _options = option.Value;
            var client = new MongoClient(_options.ConnectionString);
            _database = client.GetDatabase(_options.DataBaseName);
        }

        public async Task<TEntity?> Get<TEntity>(Guid id) where TEntity : class
        {
            var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TEntity?[]?> GetAll<TEntity>(Guid? id = null) where TEntity : class
        {
            var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
            var entities = await collection.Find(_ => true).ToListAsync();
            return entities.ToArray();
        }
        public async Task Create<TEntity>(TEntity val, Guid? parentId = null) where TEntity : class
        {
            var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
            await collection.InsertOneAsync(val);
        }

        public async Task<int> Count<TEntity>() where TEntity : class
        {
            var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
            return (int)await collection.CountDocumentsAsync(_ => true);
        }

        public async Task<bool> Delete<TEntity>(Guid id) where TEntity : class
        {
            var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            var result = await collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<bool> DeleteAll<TEntity>(Guid? id = null) where TEntity : class
        {
            var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
            var result = await collection.DeleteManyAsync(_ => true);
            return result.DeletedCount > 0;
        }
        public async Task Update<TEntity>(Guid id, TEntity updatedEntity) where TEntity : class
        {
            var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            await collection.ReplaceOneAsync(filter, updatedEntity);
        }
    }
}
