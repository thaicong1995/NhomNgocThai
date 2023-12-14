using WebApi.Models;

namespace WebApi.Sevice.Interface
{
    public interface IReveneuService
    {
        Revenue CreateReveneuForShop(Shop shop);
        Revenue GetRevenueByUser(int shopId, int userId);
    }
}
