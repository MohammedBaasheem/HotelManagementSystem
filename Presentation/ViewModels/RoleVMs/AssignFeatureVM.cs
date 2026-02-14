using Domain.Enums;

namespace Presentation.ViewModels.RoleVMs
{
    public class AssignFeatureVM
    {
        public Guid RoleId { get; set; }
        public Feature Feature { get; set; }
    }
}
