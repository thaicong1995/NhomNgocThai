using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Wallet
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Monney { get; set; } = 0;
    }
}
