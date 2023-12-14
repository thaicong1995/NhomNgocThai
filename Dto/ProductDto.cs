using WebApi.Models.Enum;

namespace WebApi.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public int ShopId { get; set; }
        public ProductStatus ProductStatus { get; set; }
    }
}
