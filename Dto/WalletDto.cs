using WebApi.Models;

namespace WebApi.Dto
{
    public class WalletDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public Wallet Wallet { get; set; }
    }
}
