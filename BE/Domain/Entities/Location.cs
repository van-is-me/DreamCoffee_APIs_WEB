using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Location : BaseEntity
    {
        public string? Address { get; set; }
        public string? City { get; set; }
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
        public virtual IList<Order> Orders { get; set; }
    }
}
