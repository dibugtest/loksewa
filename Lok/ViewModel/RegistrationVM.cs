using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.ViewModel
{
    public class RegistrationVM
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "First Name is Required.")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is Required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First Name Nepali is Required.")]
        public string FirstNameNep { get; set; }
        public string MiddleNameNep { get; set; }
        [Required(ErrorMessage = "Last Name Nepali is Required.")]
        public string LastNameNep { get; set; }
        [Required(ErrorMessage = "AD DOB is Required.")]

        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "BS DOB is Required.")]

        public string DOBNep { get; set; }
        [Required(ErrorMessage = "Sex is Required.")]
        public string Sex { get; set; }
        [Required(ErrorMessage = "Father's First Name is Required.")]

        public string FatherFirstName { get; set; }
        public string FatherMiddleName { get; set; }
        [Required(ErrorMessage = "Father's Last Name is Required.")]

        public string FatherLastName { get; set; }
        [Required(ErrorMessage = "Mother's First Name is Required.")]

        public string MotherFirstName { get; set; }
        public string MotherMiddleName { get; set; }
        [Required(ErrorMessage = "Mother's Last Name is Required.")]

        public string MotherLastName { get; set; }
        [Required(ErrorMessage = "Grand Father's First Name is Required.")]

        public string GrandFatherFirstName { get; set; }
        public string GrandFatherMiddleName { get; set; }
        [Required(ErrorMessage = "Grand Father's Last Name is Required.")]

        public string GrandFatherLastName { get; set; }
        public string SpouseFirstName { get; set; }
        public string SpouseMiddleName { get; set; }
        public string SpouseLastName { get; set; }
        [Required(ErrorMessage = "CitizenshipNo is Required.")]

        public string CitizenshipNo { get; set; }
        [Required(ErrorMessage = "Issued District is Required.")]

        public string CitizenshipValidDistrict { get; set; }
        [Required(ErrorMessage = "Issued Date is Required.")]

        public string CitizenshipValidDate { get; set; }
        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile is Required.")]
        public string Mobile { get; set; }
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

        public List<SelectListItem> SexName = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
                                                                        new SelectListItem {Text="Male",Value="Male" },
                                                                         new SelectListItem {Text="Female",Value="Female" },
                                                                         new SelectListItem { Text = "Others", Value = "Others" } };

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
