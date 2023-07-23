using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = Microsoft.Build.Framework.RequiredAttribute;

namespace SheetSeller.Models.DTO
{
    public class RegistrationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        //[RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$", ErrorMessage = "Minimum length 6 and must contain  1 Uppercase,1 lowercase and 1 digit")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string? Role { get; set; }
    }
}
