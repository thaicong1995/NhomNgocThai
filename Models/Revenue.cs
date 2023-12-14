using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Revenue
    {
        [Key]
        public int Id { get; set; }
        public int ShopId { get; set; }
        public decimal Monney_Reveneu { get; set; } = 0;
    }
}
