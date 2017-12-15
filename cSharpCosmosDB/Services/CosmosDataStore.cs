using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using cSharpCosmosDB.Models;
using MongoDB.Driver;

namespace cSharpCosmosDB.Services
{
    public class CosmosDataStore
    {
        private static string dsn = @"mongodb://or-aspire:HMLzWr2YUVsRciAqmra9uKEYTdnOdBxVmRcKrQPdSIYXVC1YzRUbEZbPyGoUn3mj1PyDAjeMXzhhWtrM6psx8Q==@or-aspire.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
        private static string databaseName = "Aspire_Dev";
        private static string bookCollection = "BookStore";
        private static string meetingCollection = "Meeting";
        private static IMongoClient mongoClient = null;
        private static IMongoDatabase repo = null;
        private static Random random = new Random();

        public CosmosDataStore()
        {
            Debug.WriteLine("Initializing Cosmos DB!");
            Trace.WriteLine("Initializing Cosmos DB! (trace)");

            if (repo == null)
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(dsn));
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
                mongoClient = new MongoClient(settings);
                repo = mongoClient.GetDatabase(databaseName);
            }
        }

        public async Task StackOverflow()
        {
            //string dsn = "mongodb://myusername:mypassword@mycosmosname.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            //string databaseName = "mydatabasename";

            Debug.WriteLine("Initializing Cosmos DB!");
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(dsn));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            IMongoClient mongoClient = new MongoClient(settings);
            IMongoDatabase db = mongoClient.GetDatabase(databaseName);

            var databases = (await mongoClient.ListDatabasesAsync()).ToList();
            foreach (var d in databases)
            {
                Debug.WriteLine(d.AsBsonDocument);
            }
        }


        public async Task<bool> ConnectTest()
        {
            var databases = (await mongoClient.ListDatabasesAsync()).ToList();
            foreach (var db in databases)
            {
                System.Diagnostics.Trace.WriteLine(db.AsString);
            }
            return true;
        }

        public async Task<Meeting> GenerateMeeting()
        {
            var collection = repo.GetCollection<Meeting>(meetingCollection);

            var code = await GenerateMeetingCode(5, collection);
            var meeting = new Meeting() { Code = code };
            await collection.InsertOneAsync(meeting);
            return meeting;
        }

        public async Task<string> GenerateMeetingCode(int length = 5, IMongoCollection<Meeting> collection = null)
        {
            if (collection == null)
            {
                collection = repo.GetCollection<Meeting>(meetingCollection);
            }

            var code = "";
            bool codeIsUnique = false;
            while (!codeIsUnique)
            {
                //Generate Code
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                code = new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                var count = await collection.CountAsync(m => m.Code == code);
                codeIsUnique = count == 0;
            }

            return code;
        }

        public async Task<string> SaveBook()
        {
            var collection = repo.GetCollection<BookStore>(bookCollection);

            BookStore bookStore = new BookStore
            {
                BookTitle = "MongoDB Basics",
                ISBN = "8767687689898yu",
                Auther = "Tanya",
                Category = "NoSQL DBMS"
            };

            //collection.Save(bookStore);
            await collection.InsertOneAsync(bookStore);
            return bookStore.Id.ToString();
        }

        public async Task<long> BookCount()
        {
            var collection = repo.GetCollection<BookStore>(bookCollection);
            var cnt = await collection.CountAsync(c => true);
            return cnt;
        }

        public async Task<long> MeetingCount()
        {
            var collection = repo.GetCollection<Meeting>(meetingCollection);
            var cnt = await collection.CountAsync(c => true);
            return cnt;
        }

        public async Task DeleteAllMeetings()
        {
            var collection = repo.GetCollection<Meeting>(meetingCollection);
            await collection.DeleteManyAsync(m => true);
        }
    }
}
