using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.ViewModel
{
    public class UploadVM
    {
        public string Id { get; set; }
        public string Photograph { get; set; }
        public string Signature { get; set; }
        public string Citizenship { get; set; }
        public string InclusionGroupp { get; set; }
        public string ExperienceDocument { get; set; }
    }
}
