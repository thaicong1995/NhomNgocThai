using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Sevice.Interface
{
    public interface IDiscountService
    {
        Discounts CreateDiscount(DiscountDTo discount);
        decimal ApplyDiscount(Order order, int discountId, int userId);
        bool IsDiscountUsedByUser(int userId, int discountId);
        void SaveDiscountByUserId(int userId, int discountId);

        List<Discounts> GetAllDiscount();
    }
}
