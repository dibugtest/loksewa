using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Models
{
    public class Applications
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Applicant { get; set; }
        public string Advertisement { get; set; }
        public string[] EthnicalGroup { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentFileName { get; set; }
        public string MinQualification { get; set; }
        public string Faculty { get; set; }
        public string MainSubject { get; set; }
        public string ExamCenter { get; set; }
        public DateTime AppliedDate { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime VerifiedDate { get; set; }
        public Decimal TotalAmount { get; set; }
    }
}
