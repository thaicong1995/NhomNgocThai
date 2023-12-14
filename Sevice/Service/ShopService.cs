
using WebApi.Dto;
using WebApi.Models;
using WebApi.Models.Enum;
using WebApi.MyDbContext;
using WebApi.Reposetory.Interface;
using WebApi.Sevice.Interface;

namespace WebApi.Sevice.Service
{
    public class ShopService : IShopService
    {
        private readonly MyDb _myDb;
        private readonly IReveneuService _iRevenueService;
        private readonly IRepository _iRepository;
        public ShopService(MyDb myDb, IReveneuService revenueService, IRepository repository)
        {
            _myDb = myDb;
            _iRevenueService = revenueService;
            _iRepository = repository;
        }

        public List<Shop> GetShops()
        {
            try
            {
                List<Shop> shops = _myDb.Shops.ToList();

                return shops;
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }

        public Shop CreateShopForUser(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User information is missing.");
                }

                // Tạo một cửa hàng mới
                Shop newShop = new Shop
                {
                    UserId = user.Id, // Đặt UserId bằng ID của người dùng
                    _shopStatus = ShopStatus.InActive // Đặt trạng thái cửa hàng là InActive
                };

                _myDb.Shops.Add(newShop);
                _myDb.SaveChanges();

                return newShop;
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }

        public bool ActivateShop(int userId, ShopDto shopDto)
        {
            try
            {
                Shop shop = _iRepository.GetShopByUser(userId);

                if (shop == null)
                {
                    throw new ArgumentNullException(nameof(shop), "Shop information is missing.");
                }


                if (shop._shopStatus == ShopStatus.InActive)
                {
                    // Cập nhật thông tin cửa hàng nếu dữ liệu hợp lệ
                    if (!string.IsNullOrEmpty(shopDto.ShopName))
                    {
                        shop.ShopName = shopDto.ShopName;
                    }

                    if (!string.IsNullOrEmpty(shopDto.Address))
                    {
                        shop.Address = shopDto.Address;
                    }

                    if (!string.IsNullOrEmpty(shopDto.Phone))
                    {
                        shop.Phone = shopDto.Phone;
                    }

                    shop.Create = DateTime.Now;
                    // Đặt trạng thái cửa hàng thành Active
                    shop._shopStatus = ShopStatus.Active;
                    _iRevenueService.CreateReveneuForShop(shop);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _myDb.SaveChanges();

                    return true; // Cập nhật thành công
                }
                else
                {
                    // Nếu shop đã được kích hoạt thì không thể active lại
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string UpdateShop(int userId, ShopDto shopDto)
        {
            try
            {
                Shop shop = _iRepository.GetShopByUser(userId);

                if (shop == null)
                {
                    throw new ArgumentNullException(nameof(shop), "Shop information is missing.");
                }

                if (shop._shopStatus == ShopStatus.Active)
                {
                    // Cập nhật thông tin cửa hàng nếu dữ liệu hợp lệ
                    if (!string.IsNullOrEmpty(shopDto.ShopName))
                    {
                        shop.ShopName = shopDto.ShopName;
                    }

                    if (!string.IsNullOrEmpty(shopDto.Address))
                    {
                        shop.Address = shopDto.Address;
                    }

                    if (!string.IsNullOrEmpty(shopDto.Phone))
                    {
                        shop.Phone = shopDto.Phone;
                    }

                    shop.Update = DateTime.Now;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _myDb.SaveChanges();

                    return "Updated success !";
                }
                else
                {
                    // Nếu shop chưa được kích hoạt --> thông báo
                    return "You need active Shop";
                }
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");  // Lỗi xảy ra
            }

        }


        public List<OrderDetails> GetProductSold(int shopId, int userId)
        {
            try
            {
                var shopExists = _iRepository.CheckshopByUserId(userId, shopId);
                if (shopExists)
                {
                    Console.WriteLine("Shop exists for the user.");
                    var ProductSold = _iRepository.ProductSold(shopId);

                    return ProductSold;
                }

                throw new Exception("Invalid UserId or shop does not exist for the user.");

            }
            catch (Exception ex)
            {
                throw new Exception($"Invalid  product does not exist. + {ex}");
            }
        }

        public Shop GetinforById (int shopId)
        {
            try
            {
                var shopInfo = _iRepository.GetShopByID(shopId);
                return shopInfo;
            }catch (Exception ex)
            {
                throw new Exception($"Invalid  shop does not exist. + {ex}");
            }
        }
    }
}
