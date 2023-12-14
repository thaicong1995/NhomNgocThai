using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApi.Models.Enum;
namespace WebApi.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipPhone { get; set; }

        public decimal PriceQuantity { get; set; }
        public decimal TotalPrice { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int ShopId { get; set; }
        public int UserId { get; set; }
        public string OrderNo { get; set; }
        [JsonPropertyName("OrderDate")]
        [JsonIgnore]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime RefunTime { get; set; }
        public PayMethod payMethod { get; set; }

        public string PayMethodString
        {
            get
            {
                return payMethod.ToString();
            }
        }
        public int? DiscountId { set; get; }    
        public OrderStatus _orderStatus { get; set; }
        public bool IsReveneu { set; get; } = false;

        public string OrderStatusString
        {
            get
            {
                return _orderStatus.ToString();
            }
        }
    }
}
