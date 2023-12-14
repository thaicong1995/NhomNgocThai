using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Reposetory.Interface
{
    public interface IRepository
    {

        //user
        public User GetUserById(int userId);
        User GetUserByActivationToken(string activationToken);
        User GetUserByEmail(string email);
        AcessToken GetValidTokenByUserId(int userId);

        //shop
        Shop GetShopByUser(int userId);
        Shop GetShopByID(int Id);
        Shop FindShopByKeyword(string keyword);
        bool IsShopActive(int shopId);
        List<OrderDetails> ProductSold(int shopId);

        bool CheckshopByUserId(int userId, int shopId);

        //revenneu
        Revenue ReveneuByShop(int shopId);

        //product
        Product GetProductById(int productId);
        List<Product> FindProductsByKeyword(string keyword);
        Product GetProductByID(int productId);
        List<Product> GetProductsByShop(int shopId);

        //cart
        CartItem GetCart(int productId);
        CartItem GetCartItemByUser(int productId, int userId);
        List<CartItem> GetSelectedCartItemsByUserId(int userId);

        //Order
        List<Order> GetOrdersByOrderNo(string orderNo, int userId);
        Order GetProductInOrder(string orderNo, int productId);
        List<Order> HistoryBuy(int userId);

        //wallet
        Wallet GetWalletByUserId(int userId);
    }
}
