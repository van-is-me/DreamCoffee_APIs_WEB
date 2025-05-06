using Application.ViewModels.PaymentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentAsync(string gateway, PaymentRequestViewModel paymentRequestViewModel);
    }
}
