using Microsoft.AspNetCore.Identity;

namespace _3D_XO_Server.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public bool? IsActive { get; set; }
        public int Violation { get; set; }
    }
}
