using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.MomoViewModels
{

    public class MomoRequestViewModel //Lớp này mô tả một yêu cầu API gửi đến Momo để thực hiện một giao dịch thanh toán.
    {
        public string partnerCode { get; set; } //mã đối tác
        public string accessKey { get; set; } //khóa truy cập
        public string requestId { get; set; } //mã yêu cầu
        public string amount { get; set; } //số tiền thanh toán
        public string orderId { get; set; }//mã đơn hàng
        public string orderInfo { get; set; }   //thông tin đơn hàng
        public string redirectUrl { get; set; } //URL chuyển hướng sau khi thanh toán
        public string ipnUrl { get; set; } // URL thông báo kết quả thanh toán
        public string extraData { get; set; } = ""; //dữ liệu bổ sung
        public string requestType { get; set; } // loại yêu cầu
        public string signature { get; set; } //chữ ký xác thực
        public string lang { get; set; } = "vi"; // ngôn ngữ
    }
}
