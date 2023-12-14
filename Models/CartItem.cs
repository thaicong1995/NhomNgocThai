using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApi.Models.Enum;

namespace WebApi.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal PriceQuantity { get; set; } = 0;
        public int ShopId { get; set; }

        [JsonPropertyName("CreateAt")]
        [JsonIgnore]
        public DateTime CreateAt { get; set; } = DateTime.Now;
        [JsonPropertyName("RefunTimeExpried")]
        [JsonIgnore]
        public int UserId { get; set; }
        public bool isSelect { get; set; }

    }
}
