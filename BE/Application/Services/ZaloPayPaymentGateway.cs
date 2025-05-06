using Application.Interfaces;
using Application.ViewModels.PaymentViewModels;
using Application.ViewModels.ZaloPayViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ZaloPayPaymentGateway : IPaymentGateway
    {
        private readonly ZaloPaySettingsViewModel _zaloPaySettings;

        public ZaloPayPaymentGateway(IOptions<ZaloPaySettingsViewModel> zaloPaySettings)
        {
            _zaloPaySettings = zaloPaySettings.Value;
        }

        public string GatewayName => "zalopay";

        public async Task<string> CreatePaymentAsync(PaymentRequestViewModel model)
        {
            if (string.IsNullOrEmpty(_zaloPaySettings.AppId) || string.IsNullOrEmpty(_zaloPaySettings.Key1) || string.IsNullOrEmpty(_zaloPaySettings.Endpoint))
                throw new ArgumentException("ZaloPay configuration is missing required values.");

            var appTransId = DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString("N").Substring(0, 8);
            var appTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var rawData = $"app_id={_zaloPaySettings.AppId}&app_trans_id={appTransId}&app_user={_zaloPaySettings.AppUser}&amount={model.Amount}&app_time={appTime}&embed_data={{}}&item=[]&description={model.OrderInfo}&bank_code=";

            string mac;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_zaloPaySettings.Key1)))
            {
                var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                mac = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
            }

            var zaloPayRequest = new ZaloPayRequestViewModel
            {
                app_id = _zaloPaySettings.AppId,
                app_trans_id = appTransId,
                app_user = _zaloPaySettings.AppUser,
                amount = model.Amount,
                app_time = appTime,
                embed_data = "{}",
                item = "[]",
                description = model.OrderInfo,
                bank_code = "",
                mac = mac
            };

            using var httpClient = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(zaloPayRequest), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(_zaloPaySettings.Endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to call ZaloPay API. Response: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Parse the response to extract the payment URL
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
            if (responseJson != null && responseJson.TryGetValue("payment_url", out var paymentUrl))
            {
                return paymentUrl.ToString(); // Return only the payment URL
            }

            throw new Exception("Payment URL not found in ZaloPay API response.");
        }
    }
}