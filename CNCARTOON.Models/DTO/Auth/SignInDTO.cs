using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNCARTOON.Models.DTO.Auth
{
    public class SignInDTO
    {
        [Required(ErrorMessage = "UserName is required")] 
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
    }
}
