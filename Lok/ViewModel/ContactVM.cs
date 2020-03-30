using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.ViewModel
{
    public class ContactVM
    {
        public string Id { get; set; }
        [Required(ErrorMessage ="District is Required.")]
        public string District { get; set; }
        [Required(ErrorMessage = "District is Required.")]
        public string State { get; set; }
        [Required(ErrorMessage = "District is Required.")]

        public string MunicipalityType { get; set; }
        [Required(ErrorMessage = "District is Required.")]

        public string MunicipalityName { get; set; }
        [Required(ErrorMessage = "District is Required.")]

        public string WardNo { get; set; }
        public string Tole { get; set; }
        public string Marga { get; set; }
        public string HouseNo { get; set; }
        public string PhoneNo { get; set; }
        [Required(ErrorMessage = "District is Required.")]

        public string MobileNo { get; set; }
        [Required(ErrorMessage = "District is Required.")]

        public string Address { get; set; }
        [Required(ErrorMessage = "District is Required.")]
        [EmailAddress(ErrorMessage ="Invalid email address.")]
        public string Email { get; set; }
        public SelectList Districts { get; set; }
        
        public List<SelectListItem> MunicipalityTypes = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
                                                                        new SelectListItem {Text="महानगरपालिका",Value="महानगरपालिका" },
                                                                         new SelectListItem {Text="उपमहानगरपालिका",Value="उपमहानगरपालिका" },
                                                                         new SelectListItem { Text = "नगरपालिका", Value = "नगरपालिका" },
                                                                         new SelectListItem { Text = "गाँउपालिका", Value = "गाँउपालिका" }
        };

        public List<SelectListItem> States = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
                                                                        new SelectListItem {Text="1",Value="1" },
                                                                         new SelectListItem {Text="2",Value="2" },
                                                                         new SelectListItem { Text = "3", Value = "3" },
                                                                         new SelectListItem { Text = "4", Value = "4" },
                                                                         new SelectListItem { Text = "5", Value = "5" },
                                                                         new SelectListItem { Text = "6", Value = "6" },
                                                                         new SelectListItem { Text = "7", Value = "7" }
        };



    }
}
