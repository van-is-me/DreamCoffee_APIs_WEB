﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? Username { get; set; } 
        public string? PasswordHash { get; set; } 
        public UserRole Role { get; set; }
        public string? Image { get; set; }

        public virtual IList<Order> Orders { get; set; }
        public virtual IList<Transaction> Transactions { get; set; }
        public virtual IList<Review> Reviews { get; set; }
        public virtual IList<Location> Locations { get; set; }
        public virtual IList<Shipping> Shippings { get; set; }
    }
}
