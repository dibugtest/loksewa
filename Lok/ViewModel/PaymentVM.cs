using Lok.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.ViewModel
{
    public class PaymentVM
    {
        public IEnumerable<ApplicationPay> Applications { get; set; }
        public List<SelectListItem> ApplicationStatus = new List<SelectListItem> {
                                                                                      new SelectListItem { Text = "--Select--", Value = "" }
                                                                                    , new SelectListItem { Text = "Pending", Value = "Pending" },
                                                                                        new SelectListItem { Text = "Completed", Value = "Completed" }
                                                                                    };
        public string Status { get; set; }

        public string Bank { get; set; }
        public List<SelectListItem> Banks= new List<SelectListItem> {
                                                                                      new SelectListItem { Text = "--Select--", Value = "" }
                                                                                    , new SelectListItem { Text = "Bank1", Value = "Bank1" },
                                                                                        new SelectListItem { Text = "Bank2", Value = "Bank2" }
                                                                                    };
        public Applicant Applicant { get; set; }
    }
    public class ApplicationPay
    {
        public Advertisiment ObjAd { get; set; }
        public string Id { get; set; }
        public string Applicant { get; set; }
        public string Advertisement { get; set; }
        public string EthnicalGroups { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentFileName { get; set; }
        public string MinQualification { get; set; }
        public string Faculty { get; set; }
        public string MainSubject { get; set; }
        public string ExamCenter { get; set; }
        public DateTime AppliedDate { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime VerifiedDate { get; set; }
        public Decimal TotalAmount { get; set; }
    }
}
