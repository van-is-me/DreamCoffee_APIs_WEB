using Application.Interfaces;
using Application.ViewModels.MomoViewModels;
using Application.ViewModels.PaymentViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MomoPaymentGateWay : IPaymentGateway
    {
        private readonly MomoSettingsViewModel _momoSettings;

        public MomoPaymentGateWay(IOptions<MomoSettingsViewModel> momoSettings)
        {
            _momoSettings = momoSettings.Value;
        }

        public async Task<string> CreatePaymentAsync(PaymentRequestViewModel model)
        {
            var orderId = Guid.NewGuid().ToString();
            var requestId = orderId;
            var requestType = "captureWallet";

            var rawHash = $"accessKey={_momoSettings.AccessKey}&amount={model.Amount}&extraData=&ipnUrl={_momoSettings.IpnUrl}&orderId={orderId}&orderInfo={model.OrderInfo}&partnerCode={_momoSettings.PartnerCode}&redirectUrl={_momoSettings.RedirectUrl}&requestId={requestId}&requestType={requestType}";

            string signature;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_momoSettings.SecretKey)))
            {
                var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawHash));
                signature = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
            }

            var momoRequest = new MomoRequestViewModel
            {
                partnerCode = _momoSettings.PartnerCode,
                accessKey = _momoSettings.AccessKey,
                requestId = requestId,
                amount = model.Amount.ToString(),
                orderId = orderId,
                orderInfo = model.OrderInfo,
                redirectUrl = _momoSettings.RedirectUrl,
                ipnUrl = _momoSettings.IpnUrl,
                requestType = requestType,
                signature = signature
            };

            using var httpClient = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(momoRequest), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(_momoSettings.Endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to call MoMo API");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent; // Return the response content (e.g., payment URL or QR code)
        }
    }
}