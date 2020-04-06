using Microsoft.AspNetCore.Http.Internal;
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
        public FormFile PhotographFile { get; set; }
        public string PhotographLink { get; set; }

        public string Signature { get; set; }
        public FormFile SignatureFile { get; set; }
        public string SignatureLink { get; set; }

        public string Citizenship { get; set; }
        public FormFile CitizenshipFile { get; set; }
        public string CitizenshipLink { get; set; }

        public string InclusionGroup { get; set; }
        public FormFile InclusionGroupFile { get; set; }
        public string InclusionGroupLink { get; set; }

        //public string ExperienceDocument { get; set; }
        //public FormFile ExperienceDocumentFile { get; set; }
        //public string ExperienceDocumentLink { get; set; }
    }
}
