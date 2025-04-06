using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNCARTOON.Models.DTO.Auth
{
    public class SignResponseDTO
    {
        /// <summary>
        /// JWT Access Token for API authorization
        /// </summary>
        [Required]
        public string AccessToken { get; set; }

        /// <summary>
        /// Refresh token to get new access token
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Optional: Token expiry in seconds
        /// </summary>
        public int? ExpiresIn { get; set; }
    }
}
