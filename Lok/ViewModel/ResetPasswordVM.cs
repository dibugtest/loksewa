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
    }
}
