using System.Text.Json.Serialization;
using WebApi.Models.Enum;

namespace WebApi.Dto
{
    public class DiscountDTo
    {
        public string Code { get; set; }
        public decimal Value { get; set; }
        public DiscountStatus _discountStatus { set; get; }
    }
}
