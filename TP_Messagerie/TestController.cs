using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Cassandra;

namespace TP_Messagerie
{
    public class TestController : Controller
    {
        private readonly Cassandra.ISession _cassandraSession;
        private readonly IMongoDatabase _mongoDatabase;

        public TestController(Cassandra.ISession cassandraSession, IMongoDatabase mongoDatabase)
        {
            _cassandraSession = cassandraSession;
            _mongoDatabase = mongoDatabase;
        }

        public IActionResult Index()
        {
            // Test Cassandra connection
            var cassandraResult = _cassandraSession.Execute("SELECT * FROM system.local");

            // Test MongoDB connection
            var mongoCollection = _mongoDatabase.GetCollection<BsonDocument>("your_collection");
            var mongoResult = mongoCollection.Find(FilterDefinition<BsonDocument>.Empty).FirstOrDefault();

            // Return results as a simple string
            return Content($"Cassandra: {cassandraResult.FirstOrDefault()?["key"]}\nMongoDB: {mongoResult}");
        }
    }
}
