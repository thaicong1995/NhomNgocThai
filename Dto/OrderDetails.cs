namespace WebApi.Dto
{
    public class OrderDetails
    {
        public string OrderNo { get; set; }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal PriceQuantity { get; set; }
        public int Quantity { get; set; }
    }
}
