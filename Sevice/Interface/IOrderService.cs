using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Sevice.Interface
{
    public interface IOrderService
    {
        List<Order> CreateOrders(List<int> selectedProductIds, int userId);
        List<Order> PayOrder(string OrderNo, OrderDto orderDto, int userId);
        void CalculateTotalPriceAndDiscount(List<Order> orders, OrderDto orderDto, int userId);
        void CalculateTotalPrice(List<Order> orders);
        Order RefundProduct(string OrderNo, int userId, int productId);
        List<Order> HistoryBuy(int userId);
    }
}
