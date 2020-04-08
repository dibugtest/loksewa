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
    public class PersonalVM
    {
        public string Id { get; set; }
        public string PId { get; set; }
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
        public List<SelectListItem> SexName = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
                                                                        new SelectListItem {Text="Male",Value="Male" },
                                                                         new SelectListItem {Text="Female",Value="Female" },
                                                                         new SelectListItem { Text = "Others", Value = "Others" } };

    }
}
