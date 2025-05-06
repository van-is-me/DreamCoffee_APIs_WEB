using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PaymentGatewayFactory
    {
        private readonly Dictionary<string, IPaymentGateway> _paymentGateways; // Dictionary to hold payment gateways dùng ánh xạ string sang sang đối tượng IPaymentGateway tương ứng 

        public PaymentGatewayFactory(IEnumerable<IPaymentGateway> paymentGateways) 
            // Constructor nhận vào danh sách các IPaymentGateway và ánh xạ tên của chúng sang đối tượng tương ứng
        {
            _paymentGateways = paymentGateways.ToDictionary(x => x.GatewayName.ToLower());
        }

        public IPaymentGateway GetPaymentGateway(string gateway)
        {
            if (_paymentGateways.TryGetValue(gateway.ToLower().Trim(), out var result))
                return result;

            throw new ArgumentException($"Thanh toán '{gateway}' không hỗ trợ. Vui lòng kiểm tra lại tên cổng thanh toán.");
        }

        // Cung cấp phương thức kiểm tra không ném ngoại lệ
        public bool TryGetPaymentGateway(string gateway, out IPaymentGateway paymentGateway)
        {
            return _paymentGateways.TryGetValue(gateway.ToLower().Trim(), out paymentGateway);
        }
    }
}
