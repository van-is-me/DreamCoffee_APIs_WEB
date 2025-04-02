using Domain.Enums;
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
        public string? Username { get; set; } // Cho phép null
        public string? PasswordHash { get; set; } // Cho phép null
        public UserRole Role { get; set; }
        public string? Image { get; set; }
        public string? Provider { get; set; } // Thêm Provider
        public string? ProviderId { get; set; } // Thêm ProviderId
        public bool OAuthLinked { get; set; } = false; // Thêm OAuthLinked, mặc định false
        public string? AccessToken { get; set; } // Thêm AccessToken
        public string? RefreshToken { get; set; } // Thêm RefreshToken
        public DateTime? TokenExpiry { get; set; } // Thêm TokenExpiry
        public virtual IList<Order> Orders { get; set; }
        public virtual IList<Transaction> Transactions { get; set; }
        public virtual IList<Review> Reviews { get; set; }
    }
}
