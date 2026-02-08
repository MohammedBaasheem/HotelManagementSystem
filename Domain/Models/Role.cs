namespace Domain.Models
{
    public class Role : BaseModel
    {
        private string _name;
        public string Name { get => _name; set => _name = value.ToUpperInvariant().Trim(); }
        public HashSet<UserRole> UserRole { get; set; } = new HashSet<UserRole>();
    }
}