using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Transaction : BaseEntity
    {
        [ForeignKey("Order")]
        public Guid? OrderId { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public virtual User User { get; set; }
        public virtual Order Order { get; set; }
    }
}
