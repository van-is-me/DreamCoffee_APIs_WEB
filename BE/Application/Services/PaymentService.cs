using Application.Interfaces;
using Application.ViewModels.PaymentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentGatewayFactory _paymentGatewayFactory;

        public PaymentService(PaymentGatewayFactory paymentGatewayFactory)
        {
            _paymentGatewayFactory = paymentGatewayFactory;
        }

        public async Task<string> CreatePaymentAsync(string gateway, PaymentRequestViewModel paymentRequestViewModel)
        {
            var paymentGateway = _paymentGatewayFactory.GetPaymentGateway(gateway);
            return await paymentGateway.CreatePaymentAsync(paymentRequestViewModel);
        }
    }
}
