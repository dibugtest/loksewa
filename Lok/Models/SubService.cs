using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Models
{
    public class SubService
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string SubServiceName { get; set; }
        public string Description { get; set; }
        public Service Service { get; set; }
        public string ServiceId { get; set; }
    }
}
