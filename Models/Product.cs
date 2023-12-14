using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApi.Models.Enum;

namespace WebApi.Models
{
    public class Product
    {
        [Key]
       public int Id { get; set; }
        
       public string? ProductName { get; set; }
        
       public string? Description { get; set; }
        
       public string? Category { get; set; }
        
        public decimal? Price { get; set; }
        
        public int? Quantity { get; set; }
        public string? ImagePath { get; set; }
        public int ShopId { get; set;}
       public ProductStatus _productStatus { get; set; }

        public string ProductStatusString
        {
            get
            {
                return _productStatus.ToString();
            }
        }
    }
}
