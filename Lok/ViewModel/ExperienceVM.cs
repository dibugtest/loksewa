using Lok.Models;
using Microsoft.AspNetCore.Http.Internal;
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
    public class ExperienceVM
    {
        public IEnumerable<GovernmentExperienceVM> GovernmentExperience { get; set; }
        public IEnumerable<NonGovernmentExperienceVM> NonGovernmentExperience { get; set; }
    }
    public class GovernmentExperienceVM
    {
        public string Id { get; set; }
        public string GId { get; set; }

        [Required(ErrorMessage = "Office Address is Required")]
        public string OfficeAddress { get; set; }
        [Required(ErrorMessage = "Office Name is Required")]

        public string OfficeName { get; set; }
        [Required(ErrorMessage = "Post is Required")]

        public string Post { get; set; }
        [Required(ErrorMessage = "Sewa is Required")]

        public string Sewa { get; set; }
        [Required(ErrorMessage = "Group is Required")]

        public string Samuha { get; set; }
        public string UpaSamuha { get; set; }
        [Required(ErrorMessage = "Level is Required")]

        public string TahaShreni { get; set; }
        [Required(ErrorMessage = "Remarks is Required")]

        public string Remarks { get; set; }
        [Required(ErrorMessage = "Start Date is Required")]

        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End Date is Required")]

        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Awastha is Required")]

        public string Awastha { get; set; }
        [Required(ErrorMessage = "Job Type is Required")]

        public string JobType { get; set; }

        public string FileName { get; set; }
        public FormFile FileMain { get; set; }
        public string FileMainLink { get; set; }

        public SelectList Sewas { get; set; }
        public SelectList ShreniTahas { get; set; }
        public List<SelectListItem> RemarkItems = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
            new SelectListItem {Text="New Appointment",Value="New Appointment" },
                                                                                new SelectListItem {Text="Promotion",Value="Promotion" },
                                                                                new SelectListItem {Text="Transfer",Value="Transfer" }
        };
        public SelectList Awasthas { get; set; }
        public List<SelectListItem> JobTypes = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
            new SelectListItem {Text="Permanent",Value="Permanent" },
                                                                                new SelectListItem {Text="Temporary",Value="Temporary" },
                                                                                new SelectListItem {Text="Contract",Value="Contract" }
        };


    }
    public class NonGovernmentExperienceVM
    {
        public string Id { get; set; }
        public string GId { get; set; }

        [Required(ErrorMessage = "Office Name is Required")]

        public string OfficeName { get; set; }
        [Required(ErrorMessage = "Post is Required")]

        public string Post { get; set; }
        [Required(ErrorMessage = "Level is Required")]

        public string Level { get; set; }
        [Required(ErrorMessage = "Job Type is Required")]

        public string JobType { get; set; }
        [Required(ErrorMessage = "Start Date is Required")]

        public DateTime JobStartDate { get; set; }
        [Required(ErrorMessage = "End Date is Required")]

        public DateTime JobEndDate { get; set; }
        public string FileName { get; set; }
        public FormFile FileMain { get; set; }
        public string FileMainLink { get; set; }

        public SelectList Levels { get; set; }
        public List<SelectListItem> JobTypes = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
                                                                          new SelectListItem {Text="Permanent",Value="Permanent" },
                                                                           new SelectListItem {Text="Temporary",Value="Temporary" },
                                                                           new SelectListItem {Text="Contract",Value="Contract" }
        };
    }

}
