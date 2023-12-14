using WebApi.Models;

namespace WebApi.Dto
{
    public class OrderDto
    {
       
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipPhone { get; set; }
        public int? DiscountId { get; set; }

    }
}
