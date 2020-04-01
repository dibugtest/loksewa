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
    public class ProfessionalCouncilVM
    {

        public string Id { get; set; }

        public string PId { get; set; }
        [Required(ErrorMessage = "Council's Name is Required")]

        public string ProviderName { get; set; }
        [Required(ErrorMessage = "Type is Required")]

        public string Type { get; set; }
        [Required(ErrorMessage = "Reg. No. is Required")]

        public string RegistrationNo { get; set; }
        
        public DateTime ValidateFrom { get; set; }
       
        public DateTime RenewDate { get; set; }
        
        public DateTime Validity { get; set; }
        public string ValidateFromNep { get; set; }
        public string RenewDateNep { get; set; }
        public string FileName { get; set; }

        public List<SelectListItem> Types = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
                                                                        new SelectListItem {Text="Permanent",Value="Permanent" },
                                                                                new SelectListItem {Text="Temporary",Value="Temporary" } };

    }
}
