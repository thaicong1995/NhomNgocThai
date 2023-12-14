using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Models.Enum;
using WebApi.MyDbContext;
using WebApi.Reposetory.Interface;
using WebApi.Sevice.Interface;

namespace WebApi.Sevice.Service
{
    public class RevenueService : IReveneuService
    {
        private readonly MyDb _myDb;
        private readonly IRepository _iRepository;

        public RevenueService(MyDb myDb, IRepository repository)
        {
            _myDb = myDb;
            _iRepository = repository;
        }

        public Revenue CreateReveneuForShop(Shop shop)
        {
            try
            {
                if (shop == null)
                {
                    throw new ArgumentNullException(nameof(shop), "Shop information is missing.");
                }

                Revenue revenue = new Revenue
                {
                    ShopId = shop.Id,
                    Monney_Reveneu = 0
                };

                _myDb.Revenues.Add(revenue);
                _myDb.SaveChanges();

                return revenue;
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }

        // check shop co thuoc Id nguoi dang nhap hay k?
        //Kiem tra shop da active chua
        //Kiem tra doanh thu co thuoc shop hay khong?
        public Revenue GetRevenueByUser(int shopId, int userId)
        {
            try
            {
                // Lấy thông tin của cửa hàng
                var shop = _iRepository.GetShopByID(shopId);

                if (shop == null)
                {
                    throw new Exception("The shop not found!");
                }

                if (shop.UserId != userId)
                {
                    throw new Exception("You do not have access to this shop!!");
                }

                if (shop._shopStatus != ShopStatus.Active)
                {
                    throw new Exception("You need to activate the shop!");
                }

                // Lấy thông tin doanh thu của cửa hàng
                var revenue = _iRepository.ReveneuByShop(shopId);

                if (revenue == null)
                {
                    throw new Exception("No revenue for this store.");
                }

                return revenue;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }

    }
}
