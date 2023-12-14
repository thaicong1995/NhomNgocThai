using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Sevice.Interface
{
    public interface IShopService
    {
        List<Shop> GetShops();
        Shop CreateShopForUser(User user);
        string UpdateShop(int userId, ShopDto shopDto);
        bool ActivateShop(int userId, ShopDto shopDto);
        List<OrderDetails> GetProductSold(int shopId, int userId);
        Shop GetinforById(int shopId);
    }
}
