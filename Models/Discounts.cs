using System.Text.Json.Serialization;
using WebApi.Models.Enum;

namespace WebApi.Models
{
    public class Discounts
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
        [JsonPropertyName("StartDate")]
        [JsonIgnore]
        public DateTime StartDate { set; get; } = DateTime.Now;
        [JsonPropertyName("EndDate")]
        [JsonIgnore]
        public DateTime EndDate { set; get; }
        public DiscountStatus _discountStatus { set; get; }
        public string DiscountStatusString
        {
            get
            {
                return _discountStatus.ToString();
            }
        }
    }
}
 