using MongoApi.Models;
using MongoDB.Driver;

namespace MongoApi.DataAccess
{
    public class MongoDataAccess
    {
        private const string ConnectionString = "mongodb://localhost:27017";
        private const string DatabaseName = "MongoApi";

        private static IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<T>(collection);
        }
        public static Task Create<T>(T Object)
        {
            var collection = ConnectToMongo<T>(typeof(T).Name);
            return collection.InsertOneAsync(Object);
        }

        public static async Task<List<T>> GetAll<T>()
        {
            var collection = ConnectToMongo<T>(typeof(T).Name);
            var results = await collection.FindAsync(_ => true);
            return results.ToList();
        }

        public static Task Update<T>(string Id, T Object)
        {
            var collection = ConnectToMongo<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq("Id", Id);
            return collection.ReplaceOneAsync(filter, Object, new ReplaceOptions { IsUpsert = true });
        }

        public static Task Delete<T>(string Id)
        {
            var collection = ConnectToMongo<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq("Id", Id);
            return collection.DeleteOneAsync(filter);
        }
    }
}
