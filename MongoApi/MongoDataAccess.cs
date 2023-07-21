using MongoApi.Models;
using MongoDB.Driver;

namespace MongoApi
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

        public static async Task<T?> FindGet<T>(string FliterValue, string FilertProp = "Id")
        {
            var collection = ConnectToMongo<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq(FilertProp, FliterValue);
            var results = await collection.FindAsync(filter);
            return results.FirstOrDefault();
        }

        public static async Task<List<T>> FindGetAll<T>(string FliterValue, string FilertProp = "Id")
        {
            var collection = ConnectToMongo<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq(FilertProp, FliterValue);
            var results = await collection.FindAsync(filter);
            return results.ToList();
        }

        public static async Task<List<T>> GetAll<T>()
        {
            var collection = ConnectToMongo<T>(typeof(T).Name);
            var results = await collection.FindAsync(_ => true);
            return results.ToList();
        }

        public static async void Update<T>(string FliterValue, T Object, string FilertProp = "Id")
        {
            var collection = ConnectToMongo<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq(FilertProp, FliterValue);

            foreach (var prop in Object.GetType().GetProperties())
            {
                var newvalue = prop.GetValue(Object);
                if (newvalue == null) continue;
                var update = Builders<T>.Update
                    .Set(prop.Name, newvalue);
                await collection.UpdateOneAsync(filter, update);
            }
        }

        public static Task Delete<T>(string FliterValue, string FilertProp = "Id")
        {
            var collection = ConnectToMongo<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq(FilertProp, FliterValue);
            return collection.DeleteOneAsync(filter);
        }
    }
}
