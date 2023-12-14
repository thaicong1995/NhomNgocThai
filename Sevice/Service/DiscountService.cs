using WebApi.Dto;
using WebApi.Models;
using WebApi.Models.Enum;
using WebApi.MyDbContext;
using WebApi.Sevice.Interface;

namespace WebApi.Sevice.Service
{
    public class DiscountService : IDiscountService
    {
        private readonly MyDb _myDb;
        public DiscountService(MyDb myDb)
        {
            _myDb = myDb;
        }
        public Discounts CreateDiscount(DiscountDTo discount)
        {
            try
            {
                if (discount == null)
                {
                    throw new ArgumentNullException(nameof(discount), "Discount information is missing.");
                }

                // Lấy ngày hiện tại
                DateTime currentDate = DateTime.Now;

                // Tạo đối tượng Discount với ngày bắt đầu là ngày hiện tại và ngày kết thúc là ngày hiện tại cộng thêm 1 ngày (1 ngày hiệu lực)
                var newDiscount = new Discounts
                {
                    Code = discount.Code,
                    Value = discount.Value,
                    StartDate = currentDate,
                    EndDate = currentDate.AddDays(1), // Hiệu lực trong 1 ngày
                    _discountStatus = DiscountStatus.Active
                };

                // Thêm đối tượng mã giảm giá vào cơ sở dữ liệu
                _myDb.Discounts.Add(newDiscount);
                _myDb.SaveChanges();

                return newDiscount; // Trả về đối tượng Discount sau khi đã thêm vào cơ sở dữ liệu
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }

        public decimal ApplyDiscount(Order order, int discountId, int userId)
        {
            var discount = _myDb.Discounts.FirstOrDefault(d => d.Id == discountId);

            if (discount == null)
            {
                throw new InvalidOperationException("Discount not found!");
            }
            var date = DateTime.Now;
            if (date < discount.EndDate && date > discount.EndDate)
            {
                return 0;
            }
            if (discount._discountStatus != DiscountStatus.Active)
            {
                return 0;
            }
            return (order.Price * order.Quantity) * discount.Value;
        }

        // Kiểm tra mã giảm giá đã dùng chưa
        public bool IsDiscountUsedByUser(int userId, int discountId)
        {
            return _myDb.DiscountUsages
                .Any(du => du.UserId == userId && du.DiscountId == discountId);
        }

        //Khoi tao du lieu khi khach hang su dung discount.
        public void SaveDiscountByUserId(int userId, int discountId)
        {
            var existingUsage = _myDb.DiscountUsages
                .FirstOrDefault(du => du.UserId == userId && du.DiscountId == discountId);

            if (existingUsage == null)
            {
                var discountUsage = new DiscountUsage
                {
                    UserId = userId,
                    DiscountId = discountId,
                    UsageDate = DateTime.Now
                };

                _myDb.DiscountUsages.Add(discountUsage);
                _myDb.SaveChanges();
            }
        }

        public List<Discounts> GetAllDiscount()
        {
            try
            {
                List<Discounts> discounts = _myDb.Discounts.ToList();

                return discounts;
            }catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }
    }
}
