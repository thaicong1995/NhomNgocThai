using WebApi.Models;

namespace WebApi.Sevice.Interface
{
    public interface IWalletService
    {
        Wallet CreateWalletForUser(User user);
    }
}
