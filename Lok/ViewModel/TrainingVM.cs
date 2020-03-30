using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.ViewModel
{
    public class TrainingVM
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Organization Name is Required")]

        public string OrganizationName { get; set; }
        [Required(ErrorMessage = "Training Name is Required")]

        public string TrainingName { get; set; }
        public string DivisionPercentage { get; set; }
        

        public DateTime StartDate { get; set; }
       
        public DateTime EndDate { get; set; }
        public string StartDateNep { get; set; }
        public string EndDateNep { get; set; }
        public string FileName { get; set; }

    }
}
