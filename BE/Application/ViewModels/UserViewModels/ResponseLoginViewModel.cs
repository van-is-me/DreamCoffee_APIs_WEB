using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserViewModels
{
    public class ResponseLoginViewModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string? Image { get; set; }
        public string Token { get; set; }
    }
}
