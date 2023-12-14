using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dto;
using WebApi.Models.Enum;
using WebApi.Sevice.Interface;
using WebApi.Sevice.Service;
using WebApi.TokenConfig;

namespace WebApi.Controllers
{
    public class AddCardController : Controller
    {
        private readonly IAddCardATMSevice _iAddCardATMSevice;

        private readonly Token _token;

        public AddCardController(IAddCardATMSevice addCardATMSevice, Token token)
        {
            _iAddCardATMSevice = addCardATMSevice;
            _token = token;
        }
        [HttpPost("Add-Card")]
        [Authorize]
        public IActionResult AddCard([FromBody] AddCardATMDTO addCardATMDTO)
        {
            try
            {
                var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userClaims != null && int.TryParse(userClaims.Value, out int userID))
                {
                    var tokenStatus = _token.CheckTokenStatus(userID);

                    if (tokenStatus == StatusToken.Expired)
                    {
                        // Token không còn hợp lệ, từ chối yêu cầu
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }
                    var addCard = _iAddCardATMSevice.AddCardATM(userID, addCardATMDTO);

                    return Ok(addCard);
                }
                else
                {
                    return BadRequest("User ID is not available in the claims.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
    }
}
