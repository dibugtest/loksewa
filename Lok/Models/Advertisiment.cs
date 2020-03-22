using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Models
{
    public class Advertisiment
    {
        public Guid Id { get; set; }
        public string AdvertisimentType { get; set; }
        public string SNo { get; set; }
        public Service Service { get; set; }
        public int ServiceId { get; set; }
        public SubService SubService { get; set; }
        public int SubServiceId { get; set; }
        public string DatePublished { get; set; }
        public string Office { get; set; }
        public string Title { get; set; }

        public string Marks { get; set; }
        public string NoticeNo { get; set; }

        public string MinAge { get; set; }
        public string MaxAge { get; set; }
        public string Examtype { get; set; }

        public string MinEduQualification { get; set; }

    }
}
