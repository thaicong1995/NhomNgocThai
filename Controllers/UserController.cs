// UserController.cs
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dto;
using WebApi.Models;
using WebApi.Sevice.Service;
using WebApi.TokenConfig;
using WebApi.Models.Enum;
using WebApi.MyDbContext;
using WebApi.Sevice.Interface;

namespace WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _iUserService;

        private readonly Token _token;
        private readonly EmailService _emailService;

        public UserController(IUserService userService, Token token, EmailService emailService)
        {
            _iUserService = userService;
            _token = token;
            _emailService = emailService;
        }


        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<string>> RegisterAsync(User user)
        {
            try
            {
                User registeredUser = _iUserService.RegisterUser(user);

                return Ok("User created successfully. Please check your email to active account");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }

        [Route("activate")]
        [HttpGet]
        public IActionResult ActivateUser([FromQuery] string activationToken)
        {
            if (string.IsNullOrEmpty(activationToken))
            {
                // Xử lý mã kích hoạt không hợp lệ
                return BadRequest(new { message = "Invalid activation token" });
            }

            // Gọi hàm ActivateUser để kích hoạt người dùng
            bool activationResult = _iUserService.ActivateUser(activationToken);

            if (activationResult)
            {
                // Kích hoạt thành công
                return Ok(new { message = "Activation successful" });
            }
            else
            {
                // Kích hoạt không thành công
                return BadRequest(new { message = "Activation failed" });
            }
        }


        [HttpPost]
        [Route("login")]
        public ActionResult<User> Login([FromBody] UserDto userDto)
        {
            try
            {
                if (userDto == null || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
                {
                    return BadRequest("Email and password are required.");
                }

                User user = _iUserService.LoginUser(userDto);
                string jwtToken = _token.CreateToken(user);
                //Phiên làm việc của Cookie 
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(3000)
                };

                HttpContext.Response.Cookies.Append("authenticationToken", jwtToken, cookieOptions);
                return Ok(jwtToken);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                // Lấy thông tin người dùng từ token
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var tokenStatus = _token.CheckTokenStatus(userId);

                    if (tokenStatus == StatusToken.Expired)
                    {
                        // Token không còn hợp lệ, từ chối yêu cầu
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }
                    // Gọi service để đăng xuất người dùng
                    var result = _iUserService.LogoutUser(userId);

                    if (result)
                    {
                        // Xóa cookei người dùng
                        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        return Ok("Logged out successfully.");
                    }
                    else
                    {
                        return StatusCode(500, "An error occurred during logout.");
                    }
                }
                else
                {
                    return BadRequest("Invalid user ID.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("Reset")]

        public IActionResult ForgotPassword([FromQuery] string email)
        {
            try
            {
                // Gọi phương thức `SendPasswordResetEmail` để gửi email đặt lại mật khẩu
                bool emailSent = _iUserService.SendPasswordResetEmail(email);

                if (emailSent)
                {
                    return Ok("Password reset email sent successfully.");
                }
                else
                {
                    return BadRequest("Email address not found or failed to send reset email.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

    }
}
