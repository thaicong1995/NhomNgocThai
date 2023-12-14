using WebApi.Models.Enum;
using WebApi.Models;
using WebApi.MyDbContext;
using WebApi.Sevice.Interface;

namespace WebApi.Sevice.Service
{
    public class WalletService : IWalletService
    {
        private readonly MyDb _myDb;

        public WalletService()
        {
        }

        public WalletService(MyDb myDb)
        {
            _myDb = myDb;
        }

        public Wallet CreateWalletForUser(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User information is missing.");
                }

                // Tạo một cửa hàng mới
                Wallet newWallet = new Wallet
                {
                    UserId = user.Id,
                };

                _myDb.Wallets.Add(newWallet);
                _myDb.SaveChanges();

                return newWallet;
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }
    }
}
