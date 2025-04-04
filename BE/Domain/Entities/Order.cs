using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [ForeignKey("Location")]
        public Guid LocationId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public StatusOrder Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public virtual User User { get; set; }
        public virtual Location? Location { get; set; }
        public virtual Transaction? Transaction { get; set; }
        public virtual Shipping? Shipping { get; set; }
        public virtual IList<OrderDetail> OrderDetails { get; set; }
    }
}
