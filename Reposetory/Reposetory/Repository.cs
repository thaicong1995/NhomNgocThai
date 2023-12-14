using Org.BouncyCastle.Asn1.X509;
using WebApi.Dto;
using WebApi.Models;
using WebApi.Models.Enum;
using WebApi.MyDbContext;
using WebApi.Reposetory.Interface;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace WebApi.Reposetory.Reposetory
{
    public class Repository : IRepository
    {
        private readonly MyDb _myDb;

        public Repository(MyDb myDb)
        {
            _myDb = myDb;
        }

        public User GetUserById(int userId)
        {
            return _myDb.Users.SingleOrDefault(s => s.Id == userId);
        }

        public User GetUserByActivationToken(string activationToken)
        {
            return _myDb.Users.SingleOrDefault(u => u.ActivationToken == activationToken);
        }

        public User GetUserByEmail(string email)
        {
            return _myDb.Users.SingleOrDefault(u => u.Email == email);
        }

        public AcessToken GetValidTokenByUserId(int userId)
        {
            return _myDb.AccessTokens.FirstOrDefault(t => t.UserID == userId && t.statusToken == StatusToken.Valid);
        }

        public Shop GetShopByUser(int userId)
        {
            return _myDb.Shops.FirstOrDefault(s => s.UserId == userId);
        }

        public Shop GetShopByID(int ShopId)
        {
            return _myDb.Shops.FirstOrDefault(s => s.Id == ShopId);
        }

        public Shop FindShopByKeyword(string keyword)
        {
            return _myDb.Shops.FirstOrDefault(shop => shop.ShopName.ToLower() == keyword);
        }

        public bool IsShopActive(int shopId)
        {
            var shop = _myDb.Shops.FirstOrDefault(s => s.Id == shopId);

            return shop != null && shop._shopStatus == ShopStatus.Active;
        }

        public bool CheckshopByUserId(int userId, int shopId)
        {
            return _myDb.Shops.Any(s => s.UserId == userId && s.Id == shopId);
        }

        public Revenue ReveneuByShop(int shopId)
        {
            return _myDb.Revenues.FirstOrDefault(r => r.ShopId == shopId);
        }

        public Product GetProductByID(int productId)
        {
            return _myDb.Products.FirstOrDefault(s => s.Id == productId);
        }

        public List<Product> GetProductsByShop(int shopId)
        {
            return _myDb.Products.ToList();
        }

        public List<OrderDetails> ProductSold(int shopId)
        {
            return _myDb.Orders
                        .Where(o => o.ShopId == shopId && o._orderStatus == OrderStatus.Success)
                        .Select(o => new OrderDetails
                        {
                            OrderNo = o.OrderNo,
                            Id = o.ProductId,
                            ProductName = o.ProductName,
                            PriceQuantity = o.PriceQuantity,
                            Quantity = o.Quantity
                        })
                        .ToList();

        }
        public List<Product> FindProductsByKeyword(string keyword)
        {
            return _myDb.Products.Where(product => product.ProductName.ToLower().Contains(keyword)).ToList();
        }


        public CartItem GetCart(int productId)
        {
            return _myDb.CartItems.FirstOrDefault(c => c.ProductId == productId && !c.isSelect);
        }

        public CartItem GetCartItemByUser(int productId, int userId)
        {
            return _myDb.CartItems.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId && !c.isSelect);
        }

        public List<CartItem> GetSelectedCartItemsByUserId(int userId)
        {
            var selectedCartItems = _myDb.CartItems
           .Where(cartItem => cartItem.UserId == userId)
           .ToList();

            foreach (var cartItem in selectedCartItems)
            {
                var existingCartItem = _myDb.CartItems.FirstOrDefault(c => c.Id == cartItem.Id);
                if (existingCartItem != null)
                {
                    existingCartItem.isSelect = true;
                }
            }

            return selectedCartItems;
        }

        public Product GetProductById(int productId)
        {
            return _myDb.Products.FirstOrDefault(p => p.Id == productId);
        }

        public List<Order> GetOrdersByOrderNo(string orderNo, int userId)
        {
            return _myDb.Orders.Where(o => o.UserId == userId && o.OrderNo == orderNo && o._orderStatus == OrderStatus.WaitPay).ToList();
        }

        public Order GetProductInOrder(string orderNo, int productId)
        {
            return _myDb.Orders.FirstOrDefault(o => o.OrderNo == orderNo && o.ProductId == productId);
        }

        public Wallet GetWalletByUserId(int userId)
        {
            return _myDb.Wallets.FirstOrDefault(w => w.UserId == userId);
        }

        public List<Order> HistoryBuy(int userId)
        {
            return _myDb.Orders
            .Where(order => order.UserId == userId && order.IsReveneu)
            .ToList();
        }
    }
}
