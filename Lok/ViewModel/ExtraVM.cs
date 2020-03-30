using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.ViewModel
{
    public class ExtraVM
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Cast is Required.")]

        public string Cast { get; set; }
        [Required(ErrorMessage = "Religion is Required.")]

        public string Religion { get; set; }
        public string OtherReligion { get; set; }
        [Required(ErrorMessage = "Marital Status is Required.")]

        public string MaritalStatus { get; set; }
        [Required(ErrorMessage = "Employment is Required.")]

        public string EmploymentStatus { get; set; }
        public string EmploymentStatusOther { get; set; }
        [Required(ErrorMessage = "Mother Language is Required.")]

        public string MotherTongue { get; set; }
        [Required(ErrorMessage = "Physical disability is Required.")]

        public string DisabilityStatus { get; set; }
        public string Disability { get; set; }
        [Required(ErrorMessage = "Father's education level is Required.")]

        public string FathersLiteracy { get; set; }
        [Required(ErrorMessage = "Mother's Education level is Required.")]

        public string MothersLiteracy { get; set; }
        [Required(ErrorMessage = "Family Occupation is Required.")]

        public string FamilyOccupation { get; set; }
        public string OtherOccupation { get; set; }

        public string NickName { get; set; }
        [Required(ErrorMessage = "Where you from is Required.")]

        public string Goegraphy { get; set; }
        [Required(ErrorMessage = "Cast group is Required.")]

        public string GroupName { get; set; }
        public string GroupNameOther { get; set; }

        public SelectList Religions { get; set; }
        public SelectList Employments { get; set; }
        public SelectList Occupations { get; set; }
        public SelectList Vargas { get; set; }

      
        public List<SelectListItem> MaritalStatuss = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
                                                                            new SelectListItem {Text="Single",Value="Single" },
                                                                         new SelectListItem {Text="Married",Value="Married" },
                                                                         new SelectListItem { Text = "Widowed", Value = "Wodowed" },
                                                                         new SelectListItem { Text = "Divorce", Value = "Divorce" } };

        public List<SelectListItem> Disabilities = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
                                                                            new SelectListItem {Text="Yes",Value="Yes" },
                                                                         new SelectListItem {Text="No",Value="No" }
                                                                          };

        public List<SelectListItem> EducationLevels = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
                                                                                new SelectListItem {Text="Uneducated",Value="Uneducated" },
                                                                                new SelectListItem {Text="Educated",Value="Educated" },
                                                                                 new SelectListItem {Text="SLC",Value="SLC" },
                                                                                  new SelectListItem {Text="Higher Education",Value="Higher Education" }
                                                                          };
        public List<SelectListItem> GeographicRegions = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
                                                                                new SelectListItem {Text="Himalayan",Value="Himalayan" },
                                                                                new SelectListItem {Text="Mountain",Value="Mountain" },
                                                                                 new SelectListItem {Text="Backward",Value="Backward" },
                                                                                  new SelectListItem {Text="Terai",Value="Terai" }
                                                                          };



    }
}
