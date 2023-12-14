using WebApi.Models;

namespace WebApi.Dto
{
    public class SearchResult
    {
        public Shop Shop { get; set; } // Đại diện cho cửa hàng liên quan đến tìm kiếm
        public Product Product { get; set; } // Đại diện cho sản phẩm liên quan đến tìm kiếm
    }
}
