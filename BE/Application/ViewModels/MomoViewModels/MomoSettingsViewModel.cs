using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.MomoViewModels
{
    public class MomoSettingsViewModel
    {
        public string PartnerCode { get; set; } // Mã đối tác
        public string AccessKey { get; set; } // Khóa truy cập
        public string SecretKey { get; set; } // Khóa bí mật
        public string RedirectUrl { get; set; } // URL chuyển hướng sau khi thanh toán
        public string IpnUrl { get; set; } // URL thông báo kết quả thanh toán
        public string RequestType { get; set; } // Loại yêu cầu
        public string Endpoint { get; set; } // Địa chỉ API của Momo

    }
}
