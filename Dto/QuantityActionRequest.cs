using Microsoft.AspNetCore.Mvc;

namespace WebApi.Dto
{
    public class QuantityActionRequest
    {
        public int productId {  get; set; }
        public int quantity { get; set; }
    }
}
