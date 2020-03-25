using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Models
{
    public class SubGroup
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string SubGroupName { get; set; }
        public string Description { get; set; }
        public Group Group { get; set; }
        public string GroupId { get; set; }

    }
}
