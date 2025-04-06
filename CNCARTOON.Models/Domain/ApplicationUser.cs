using Microsoft.AspNetCore.Identity;

namespace CNCARTOON.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Constructor
        public ApplicationUser()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
