using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class AddCardATM
    {
        [Key]
        public int Id { get; set; }
        public int userId { get; set; }
        public string BankName { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        [JsonIgnore]
        public DateTime DateAdd { get; set; }
    }
}
