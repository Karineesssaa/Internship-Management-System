using Microsoft.AspNetCore.Identity;

namespace MyMvcApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public int? InternshipGroupId { get; set; }

        public InternshipGroup? InternshipGroup { get; set; }
    }
}