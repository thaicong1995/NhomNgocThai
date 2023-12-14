using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApi.Models.Enum;

namespace WebApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [JsonPropertyName("create")]
        [JsonIgnore]
        public DateTime Create { get; set; } = DateTime.Now;
        [JsonPropertyName("update")]
        [JsonIgnore]
        public DateTime? Update { get; set; }
        [JsonIgnore]
        public UserStatus _userStatus { get; set; }

        public string? ActivationToken { get; set; } = "abc";
        public string UserStatusString
        {
            get
            {
                return _userStatus.ToString(); //thực hiện chuyển đổi dựa trên enum
            }
        }
    }
}