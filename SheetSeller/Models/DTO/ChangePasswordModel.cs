using System.ComponentModel.DataAnnotations;

namespace SheetSeller.Models.DTO
{
    public class ChangePasswordModel
    {
        [Required]
        public string? CurrentPassword { get; set; }

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Minimum length 6 and must contain  1 Uppercase,1 lowercase, 1 special character and 1 digit")]
        public string? NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string? PasswordConfirm { get; set; }
    }
    public class ChangePasswordPass
    {
        public string userID { get; set;}
        public string token { get; set;}
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Minimum length 6 and must contain  1 Uppercase,1 lowercase, 1 special character and 1 digit")]
        public string? NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string? PasswordConfirm { get; set; }
    }
    public class ChangePasswordUser 
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Username { get; set; }
    }
}