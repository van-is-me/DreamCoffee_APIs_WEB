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
        private readonly IServiceProvider _serviceProvider;

        public PaymentGatewayFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentGateway GetPaymentGateway(string gateway)
        {
            return gateway.ToLower() switch
            {
                "momo" => _serviceProvider.GetRequiredService<MomoPaymentGateWay>(),
                _ => throw new ArgumentException("Invalid payment gateway")
            };
        }
    }
}
