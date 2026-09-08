using Clinic.Domain.Entities.Identity.AuthenticationHepler;
using Microsoft.AspNetCore.Identity;

namespace Clinic.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }
        public bool? IsDelete { get; set; }
        public List<RefreshToken>? refreshTokens { get; set; }


    }

}
