using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public TypeCategory TypeCategory { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
