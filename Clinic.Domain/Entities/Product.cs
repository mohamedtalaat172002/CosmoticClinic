
using Clinic.Domain.Common;
using Clinic.Domain.Entities.Identity;

namespace BackEnd.Domain.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
