using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers.View
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordForEmailController : Controller
    {
        private readonly ResetService _resetService;
        public ResetPasswordForEmailController(ResetService resetService)
        {
            _resetService = resetService;
        }
        [HttpGet("Reset-Password")]
        public IActionResult ResetPassword([FromQuery] string token)
        {
            if (_resetService.IsValidResetToken(token))
            {

                return View("ResetPassword");
            }
            else
            {
                return View("PPP");
            }
        }

        [HttpPost("Reset-Password")]
        public IActionResult UpdatePassword([FromForm] string newPassword, [FromQuery] string token)
        {
            if (_resetService.IsValidResetToken(token))
            {
                var user = _resetService.GetUserByActivationToken(token);

                if (user != null)
                {
                    _resetService.UpdatePassword(user.Id, newPassword);

                    ViewBag.SuccessMessage = "Mật khẩu đã được đổi thành công.";

                    // Trả về trang ResetPassword với thông báo thành công
                    return View("ResetPassword");
                }
            }

            return View("ResetPassword");
        }

    }
}


