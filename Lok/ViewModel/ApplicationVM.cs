using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lok.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lok.ViewModel
{
    public class ApplicationVM
    {
        public string Id { get; set; }
        public string Advertisement { get; set; }

        public IEnumerable<Advertisiment> Advertisements { get; set; }
        [Required(ErrorMessage ="Minimum Qualification is Required.")]
        public string MinQualification { get; set; }
        [Required(ErrorMessage ="Faculty is Required.")]
        public string Faculty { get; set; }
        [Required(ErrorMessage = "Faculty is Required.")]
        public string MainSubject { get; set; }
        [Required(ErrorMessage = "Faculty is Required.")]
        public string ExamCenter { get; set; }
        
        public bool Status { get; set; }
        public string[] EthnicalGroups { get; set; }

        public List<SelectListItem> MinQualifications = new List<SelectListItem> {
            new SelectListItem { Text = "--Select--", Value = "" },
            new SelectListItem { Text = "Yes I have minimum Qualification.", Value = "Yes I have minimum Qualification." },
            new SelectListItem { Text = "No I have more than minimum Qualification.", Value = "No I have more than minimum Qualification." }
        };
        public SelectList Faculties { get; set; }
        public SelectList ExamCenters { get; set; }
        public Decimal TotalAmount { get; set; }
    }
}
