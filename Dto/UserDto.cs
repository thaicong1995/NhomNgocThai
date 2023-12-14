using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto
{
    public class UserDto
    {
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}
