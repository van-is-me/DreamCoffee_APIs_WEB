using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClaimsService : IClaimsService
    {
        //IHttpContextAccessor giúp truy cập thông tin về HTTP Request hiện tại  lấy ra các claim của người dùng đang đăng nhập
        public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.User != null)
            {//Kiểm tra xem context và thông tin người dùng có tồn tại không

                var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst("userId");
                var Id = userIdClaim?.Value;
                GetCurrentUserId = string.IsNullOrEmpty(Id) ? Guid.Empty : Guid.Parse(Id);
                //Lấy ra claim tên "userId", sau đó gán vào GetCurrentUserId
            }
            else
            {
                GetCurrentUserId = Guid.Empty;
            }
        }

        public Guid GetCurrentUserId { get; }
    }
}
