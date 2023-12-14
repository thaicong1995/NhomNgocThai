using System.ComponentModel.DataAnnotations;

public class DiscountUsage
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; } 
    public int DiscountId { get; set; }
    public DateTime UsageDate { get; set; }
}
