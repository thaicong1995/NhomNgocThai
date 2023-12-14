using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Configuration;
using WebApi.MyDbContext;
using WebApi.Sevice.Interface;

namespace WebApi.VNpay
{
    public class VnPayServiceToken
    {
        private readonly IConfiguration _configuration;
        private readonly MyDb _myDb;
        private readonly IDiscountService _iDiscountService;
        private readonly IOrderService _iOrderservice;

        public VnPayServiceToken(IConfiguration configuration, MyDb myDb, IDiscountService discountService, IOrderService orderservice)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _myDb = myDb ?? throw new ArgumentNullException(nameof(myDb));
            _iDiscountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
            _iOrderservice = orderservice ?? throw new ArgumentNullException(nameof(orderservice));
        }

        public string GenerateTokenizationUrl(int userId, HttpContext context)
        {
            var timeNow = DateTime.UtcNow;
            var tick = timeNow.Ticks.ToString();

            string vnpVersion = _configuration["Vnpay1:Version"];
            string vnpCommand = _configuration["Vnpay1:Command"];
            string vnpTmnCode = _configuration["Vnpay1:TmnCode"];
            string vnpLocale = _configuration["Vnpay:Locale"];
            string vnp_cancel_url = "https://sandbox.vnpayment.vn/token-web-demo/payment-cancel";
            string vnp_return_url = "https://sandbox.vnpayment.vn/token-web-demo/return-card-add";
            // Validate configuration values
            if (string.IsNullOrEmpty(vnpVersion) || string.IsNullOrEmpty(vnpCommand) || string.IsNullOrEmpty(vnpTmnCode) || string.IsNullOrEmpty(vnpLocale))
            {
                throw new InvalidOperationException("Error: Some configuration values are missing or empty.");
            }

            var pay = new VnPayLibraryToken(); // Assuming this class exists and has appropriate methods
            
            pay.AddRequestData("vnp_version", vnpVersion);
            pay.AddRequestData("vnp_command", vnpCommand); 
            pay.AddRequestData("vnp_tmn_code", vnpTmnCode);
            pay.AddRequestData("vnp_app_user_id", $"{userId}");
            pay.AddRequestData("vnp_bank_code", "NCB");
            pay.AddRequestData("vnp_card_type", "01");
            pay.AddRequestData("vnp_txn_ref", timeNow.ToString("yyyyMMddHHmmssfff"));
            pay.AddRequestData("vnp_txn_desc", "Thanh toan");
            pay.AddRequestData("vnp_return_url", $"{vnp_return_url}");
            pay.AddRequestData("vnp_cancel_url", $"{vnp_cancel_url}");
            pay.AddRequestData("vnp_ip_addr", $"{pay.GetIpAddress(context)}");
            pay.AddRequestData("vnp_create_date", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_secure_hash", tick);
            var sortedList = pay.GetRequestData();
            var dictionary = sortedList.ToDictionary(pair => pair.Key, pair => pair.Value); // Convert to Dictionary
            var queryString = GetQueryString(dictionary);
            var completeUrl = $"https: String";


            return completeUrl;
        }

        private string GetQueryString(Dictionary<string, string> requestData)
        {
            return string.Join("&", requestData.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
        }
    }
}
