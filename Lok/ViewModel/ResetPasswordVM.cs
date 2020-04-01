using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.ViewModel
{
    public class ResetPasswordVM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Reset Password Required.")]
        public string RandPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Security Question is Required.")]
        public string Question { get; set; }

        public List<SelectListItem> Questions = new List<SelectListItem> {new SelectListItem{Text="--Select--",Value="" },
                                                                        new SelectListItem {Text="What is your nick Name?",Value="What is your nick Name?" },
                                                                         new SelectListItem {Text="What is favorite class teacher's name in school?",Value="What is favorite class teacher's name in school?" },
                                                                         new SelectListItem {Text="What is favorite book?",Value="What is favorite book?" },
                                                                         new SelectListItem { Text = "What is your favorite place to visit?", Value = "What is your favorite place to visit?" } };
        [Required(ErrorMessage = "Security Answer is Required.")]
        public string Answer { get; set; }

    }
}
