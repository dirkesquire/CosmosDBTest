using System;
using MongoDB.Bson;

namespace cSharpCosmosDB.Models
{
    public class Meeting
    {
        public ObjectId Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
