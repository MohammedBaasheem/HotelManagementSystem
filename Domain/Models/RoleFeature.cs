using Domain.Enums;
namespace Domain.Models
{
    public class RoleFeature : BaseModel
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public Feature Feature { get; set; }
    }
}