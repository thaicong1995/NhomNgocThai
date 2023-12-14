using WebApi.Models;

namespace WebApi.Dto
{
    public class ProductWithImage
    {
        public Product ProductInfo { get; set; }
        public string ImageBytes { get; set; }
    }
}
