using System.ComponentModel.DataAnnotations;
using WebApi.Models.Enum;

namespace WebApi.Models
{
    public class AcessToken
    {
        [Key]
        public int Id { get; set; }
        public int UserID {  get; set; }
        public string AccessToken { get; set; }

        public StatusToken statusToken { get; set; }

        public DateTime ExpirationDate { get; set; } = DateTime.Now;

        public string TokenString
        {
            get
            {
                return statusToken.ToString();
            }
        }

    }
}
