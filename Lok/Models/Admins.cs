using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Models
{
    public class Admins
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
    }
}
