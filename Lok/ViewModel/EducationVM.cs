using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.ViewModel
{
    public class EducationVM
    {
       
        public string Id { get; set; }
        [Required(ErrorMessage ="Board Name is Required")]
        public string BoardName { get; set; }
        [Required(ErrorMessage = "Education Level is Required")]

        public string Level { get; set; }
        [Required(ErrorMessage = "Faculty Required")]

        public string Faculty { get; set; }
        [Required(ErrorMessage = "Percentage/Division is Required")]

        public string DivisionPercentage { get; set; }
        [Required(ErrorMessage = "Main Subject is Required")]

        public string MainSubject { get; set; }
        [Required(ErrorMessage = "Degree Name is Required")]

        public string DegreeName { get; set; }
        [Required(ErrorMessage = "Education Type is Required")]

        public string EducationType { get; set; }
        [Required(ErrorMessage = "Complition Date is Required")]

        public string CompletedDate { get; set; }
        public string DateType { get; set; }
        public string FileName { get; set; }
        public string EquivalentFileName { get; set; }

        public SelectList BoardNames { get; set; }
        public SelectList EducationLevels { get; set; }

        public SelectList Faculties { get; set; }
        public List<SelectListItem> EducationTypes = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
                                                                                new SelectListItem {Text="Government",Value="Government" },
                                                                                new SelectListItem {Text="Non-government",Value="Non-government" } };

    }
}
