using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Shipping : BaseEntity
    {
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }       
        public Guid? UserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public ShippingStatus Status { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public virtual Order Order { get; set; }
        public virtual User User { get; set; }
    }
}
