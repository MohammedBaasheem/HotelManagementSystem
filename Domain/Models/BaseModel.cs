using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}