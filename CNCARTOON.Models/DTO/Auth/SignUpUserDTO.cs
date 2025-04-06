
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNCARTOON.Models.DTO.Auth
{
    public class SignUpUserDTO
    {
        [Required(ErrorMessage = "User name is required.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]{5,}$",
            ErrorMessage = "UserName must be start a letter and at least 6 characters long.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*(),.?\""|<>{}])(?=.*\d).{6,}$",
            ErrorMessage = "Password must be at least 6 characters long, at least 1 number and contain at least one uppercase letter, one special character, and one number.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [NotMapped]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; } = null!;


    }
}
