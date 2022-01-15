using Microsoft.AspNetCore.Identity;

namespace HospitalService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}