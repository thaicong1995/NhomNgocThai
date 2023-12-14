using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Dto;
using WebApi.Models;
using WebApi.Models.Enum;
using WebApi.MyDbContext;

namespace WebApi.TokenConfig
{
    public class Token
    {
        private readonly MyDb _myDb; 
        private readonly IConfiguration _configuration;
        public Token (IConfiguration configuration, MyDb myDb)
        {
            _configuration = configuration;
            _myDb = myDb;
        }
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),

        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token256").Value!)); // Sử dụng khóa 256 bit
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime now = DateTime.Now; // Lấy thời gian hiện tại
            int expirationMinutes = 30; // Đặt thời gian hết hạn là 3 phút
            DateTime expiration = now.AddMinutes(expirationMinutes); // Tính thời gian hết hạn

            var token = new JwtSecurityToken(claims: claims, expires: expiration,
                signingCredentials: cred, issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"]);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public WalletDto GetUserWithWallet(ClaimsPrincipal principal)
        {
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var user = _myDb.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    var wallet = _myDb.Wallets.FirstOrDefault(w => w.UserId == userId);

                    return new WalletDto
                    {
                        UserId = user.Id,
                        Name = user.Name,
                        Wallet = wallet
                    };
                }
            }

            return null;
        }


        public ClaimsPrincipal DecodeToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token256").Value!));

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var nameClaim = principal.FindFirst(ClaimTypes.Name);
                Console.WriteLine(principal);
                return principal;
            }
            
            catch (Exception ex)
            {
                return null;
            }

        }

        // check token logout
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Token256"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                return principal;
            }
            catch (Exception)
            {
                // Trả về null nếu token không hợp lệ
                return null;
            }
        }
        public StatusToken CheckTokenStatus(int userId)
        {
            // Tìm Giá trị của token theo UserId
            var userTokens = _myDb.AccessTokens
                .Where(t => t.UserID == userId && t.statusToken == StatusToken.Valid)
                .OrderByDescending(t => t.ExpirationDate)
                .FirstOrDefault();

            if (userTokens != null)
            {
                //ExpirationDate > DateTime.UtcNow thì token còn hạn.
                if (userTokens.ExpirationDate > DateTime.UtcNow)
                {
                    return StatusToken.Valid;
                }
            }

            return StatusToken.Expired;
        }

    }
}
