using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Models
{
    public class Advertisiment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public ObjectId Id { get; set; }
        public string SNo { get; set; }

        public string AdvertisimentNo { get; set; }
        public string AdvertisimentType { get; set; }
        public Post Post { get; set; }
        public string PostId { get; set; }
        public Service Service { get; set; }
        public string ServiceId { get; set; }
        public Group Group { get; set; }
        public string GroupId { get; set; }
        public SubGroup SubGroup { get; set; }
        public string SubGroupId { get; set; }
        public Category Category { get; set; }
        public string CategoryId { get; set; }

        public string DatePublished { get; set; }
        public  string TimePublished { get; set; }
        public string Office { get; set; }
        public string Title { get; set; }

        public string Marks { get; set; }
        public string NoticeNo { get; set; }

        public string MinAge { get; set; }
        public string MaxAge { get; set; }
        public string Examtype { get; set; }

        public string MinEduQualification { get; set; }

        public string Notice { get; set; }
        public IList<EthinicalGroup> EthinicalGroups { get; set; }
        public IList<AdvAndEth> AdvAndEths { get; set; }
        public String CreatedDate { get; set; }
    }
}
