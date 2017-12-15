using System;
using MongoDB.Bson;

namespace cSharpCosmosDB.Models
{
    public class BookStore
    {
        public ObjectId Id { get; set; }
        public string BookTitle { get; set; }
        public string Auther { get; set; }
        public string Category { get; set; }
        public string ISBN { get; set; }
    }
}
