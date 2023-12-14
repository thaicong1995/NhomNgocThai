using WebApi.Models.Enum;
using WebApi.MyDbContext;

public class BackGroundService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BackGroundService> _logger;
    private Timer _timer;
    private bool _disposed = false;
    private readonly object _lock = new object();

    public BackGroundService(IServiceProvider serviceProvider, ILogger<BackGroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5)); // Kiểm tra mỗi 5s
        _timer = new Timer(UpdateRevenue, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    private void UpdateRevenue(object state)
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                if (scopedServices == null)
                {
                    return;
                }

                var scopedMyDb = scopedServices.GetRequiredService<MyDb>();

                DateTime now = DateTime.Now;

                _logger.LogInformation("Cập nhật doanh thu vào: " + now.ToString());

                // Lấy tất cả các đơn hàng có `RefunTime` tương ứng với thời gian hiện tại

                var ordersToUpdate = scopedMyDb.Orders
                    .Where(order => order.RefunTime <= now && order._orderStatus == OrderStatus.Success && !order.IsReveneu)
                    .ToList();

                foreach (var order in ordersToUpdate)
                {  
                    // Cập nhật doanh thu cho cửa hàng
                    var shop = scopedMyDb.Revenues.FirstOrDefault(shop => shop.ShopId == order.ShopId);
                    if (shop != null)
                    {
                        shop.Monney_Reveneu += order.TotalPrice;
                    }

                    // Đánh dấu rằng đã cập nhật doanh thu cho đơn hàng này
                    order.IsReveneu = true;
                }

                scopedMyDb.SaveChanges();
            }
        }
    }
    private void DoWork(object state)
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                if (scopedServices == null)
                {
                    return;
                }

                var scopedMyDb = scopedServices.GetRequiredService<MyDb>();

                DateTime now = DateTime.Now;

                _logger.LogInformation("Kiểm tra token hết hạn vào: " + now.ToString());

                // Lấy tất cả các token hết hạn và có trạng thái "Valid"
                var expiredTokens = scopedMyDb.AccessTokens
                    .Where(token => token.ExpirationDate <= now && token.statusToken == StatusToken.Valid)
                    .ToList();

                foreach (var token in expiredTokens)
                {
                    // Cập nhật trạng thái của token thành "Expired"
                    token.statusToken = StatusToken.Expired;
                }

                scopedMyDb.SaveChanges();
            }
        }
    }



    public Task StopAsync(CancellationToken stoppingToken)
    {
        lock (_lock)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _disposed = true; // Đánh dấu rằng đã giải phóng
        }
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        lock (_lock)
        {
            _timer?.Dispose();
        }
    }
}
