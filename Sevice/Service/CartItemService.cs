using WebApi.Dto;
using WebApi.Models;
using WebApi.Models.Enum;
using WebApi.MyDbContext;
using WebApi.Reposetory.Interface;
using WebApi.Sevice.Interface;

namespace WebApi.Sevice.Service
{
    public class CartItemService : ICartItemService
    {
        private readonly MyDb _myDb;
        private readonly IRepository _iRepository;
        public CartItemService(MyDb myDb, IRepository repository)
        {
            _myDb = myDb;
            _iRepository = repository;
        }

        //Công việc cần làm: Kiểm tra số lượng trước khi lưu vào giỏ hàng (hoàn thành)
        public List<CartItem> AddToCart(CartItemRequest cartItemRequest, int userID)
        {
            try
            {
                List<CartItem> cartItems = new List<CartItem>();

                for (int i = 0; i < cartItemRequest.ProductId.Count; i++)
                {
                    int productId = cartItemRequest.ProductId[i];
                    int quantity = cartItemRequest.quantities[i];

                    // Kiem tra san pham trong gio hang
                    CartItem productInCart = _iRepository.GetCartItemByUser(productId, userID);

                    if (productInCart != null)
                    {
                        // San pham ton tai cap nhat so luong
                        productInCart.Quantity ++;
                        productInCart.PriceQuantity = productInCart.Price * productInCart.Quantity;
                    }
                    else
                    {
                        // San pham chua co- them moi
                        Product product = _iRepository.GetProductByID(productId);
                        if (product != null)
                        {
                            if (quantity <= 0)
                            {
                                quantity = 1;
                            }

                            CartItem cartItem = new CartItem
                            {
                                ProductId = product.Id,
                                ProductName = product.ProductName,
                                Price = (decimal)(product.Price ?? 0),
                                Quantity = quantity,
                                PriceQuantity = (product.Price ?? 0) * quantity,
                                UserId = userID,
                                ShopId = product.ShopId,
                                isSelect = false
                            };

                            if (product.Quantity < cartItem.Quantity)
                            {
                                throw new Exception("Out of Stock: " + product.ProductName);
                            }

                            _myDb.CartItems.Add(cartItem);
                            productInCart = cartItem; 
                        }
                    }
                    cartItems.Add(productInCart); 
                }

                _myDb.SaveChanges();

                return cartItems; 
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }


        public CartItem IncreaseQuantity(int productId, int userId)
        {
            try
            {
                CartItem cartItem =_iRepository.GetCartItemByUser(productId, userId);

                if (cartItem != null)
                {
                
                    cartItem.Quantity ++;
                    cartItem.PriceQuantity = cartItem.Price * cartItem.Quantity;

                    // Cập nhật số lượng trong giỏ hàng
                    _myDb.SaveChanges();
                }

                return cartItem;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }

        public CartItem DecreaseQuantity(int productId, int userId)
        {
            try
            {
                CartItem cartItem = _iRepository.GetCartItemByUser(productId, userId);

                if (cartItem != null)
                {
                    cartItem.Quantity --;

                    if (cartItem.Quantity <= 0)
                    {
                        // san pham <=0 remove
                        _myDb.CartItems.Remove(cartItem);
                    }
                    else
                    {
                        cartItem.PriceQuantity = cartItem.Price * cartItem.Quantity;
                    }

                    _myDb.SaveChanges();
                }

                return cartItem;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }

    
        public bool DeleteCart(int productId, int userId)
        {
            try
            {
                CartItem cartItem = _iRepository.GetCartItemByUser(productId, userId);
                if (cartItem != null)
                {
                    _myDb.CartItems.Remove(cartItem);
                    int resault = _myDb.SaveChanges();

                    if (resault > 0)
                    {
                        return true; 
                    }
                }
                return false; 
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }


        public List<CartItem> GetCartByUerId(int userId)
        {
            try
            {
                List<CartItem> cartItems = _myDb.CartItems.Where(c => c.UserId == userId && !c.isSelect).ToList();
                return cartItems;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }

      
    }
}