using WebApi.Models;

namespace WebApi.Sevice.Interface
{
    public interface ICartItemService
    {
        List<CartItem> AddToCart(CartItemRequest cartItemRequest, int userID);
        CartItem IncreaseQuantity(int productId, int userId);
        CartItem DecreaseQuantity(int productId, int userId);
        bool DeleteCart(int productId, int userId);
        List<CartItem> GetCartByUerId(int userId);
    }
}
