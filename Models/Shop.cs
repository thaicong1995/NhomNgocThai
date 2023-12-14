using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApi.Models.Enum;

namespace WebApi.Models
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? ShopName { get; set; }
        public string? Phone { get; set; }
        [JsonPropertyName("create")]
        [JsonIgnore]
        public DateTime? Create { get; set; } = DateTime.Now;
        public DateTime? Update { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public ShopStatus _shopStatus { get; set; }

        public string ShopStatusString
        {
            get
            {
                return _shopStatus.ToString(); 
            }
        }
    }
}
