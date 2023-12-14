using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dto;
using WebApi.Models.Enum;
using WebApi.MyDbContext;
using WebApi.Sevice.Interface;
using WebApi.Sevice.Service;
using WebApi.TokenConfig;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly Token _token;
        private readonly MyDb _myDb;
        private readonly IShopService _iShopService;
        private readonly IConfiguration _configuration;
        public ShopController(Token token, MyDb myDb, IShopService shopService, IConfiguration configuration)
        {
            _token = token;
            _myDb = myDb;
            _iShopService = shopService;
            _configuration = configuration;
        }


        [Authorize]
        [HttpPost("activate-shop")]
        public IActionResult ActivateShop([FromBody] ShopDto shopDto)
        {
            try
            {
                // Lấy userID của người dùng từ Claims trong token
                var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userClaims != null && int.TryParse(userClaims.Value, out int userID))
                {
                    var tokenStatus = _token.CheckTokenStatus(userID);

                    if (tokenStatus == StatusToken.Expired)
                    {
                        // Token không còn hợp lệ, từ chối yêu cầu
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }

                    bool result = _iShopService.ActivateShop(userID, shopDto);

                    if (result)
                    {
                        return Ok("Successfully Active..");
                    }
                    else
                    {
                        return BadRequest("Cannot be activated or already activated.??");
                    }
                }
                else
                {
                    // Sai thông tin userID hoặc không có userID trong token
                    return BadRequest(new { message = "Invalid UserId." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpPut("update-shop")]
        public IActionResult UpdateShop([FromBody] ShopDto shopDto)
        {
            try
            {
                // Lấy userID của người dùng từ Claims trong token
                var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userClaims != null && int.TryParse(userClaims.Value, out int userID))
                {
                    var tokenStatus = _token.CheckTokenStatus(userID);

                    if (tokenStatus == StatusToken.Expired)
                    {
                        // Token không còn hợp lệ, từ chối yêu cầu
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }
                    string result = _iShopService.UpdateShop(userID, shopDto);
                    if (result == "Updated success !")
                    {
                        return Ok("Updated success !");
                    }
                    else
                    {
                        return BadRequest("You need active Shop");
                    }
                }
                else
                {
                    // Sai thông tin userID hoặc không có userID trong token
                    return BadRequest(new { message = "Invalid UserId." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpGet("GetShopinfo")]
        public IActionResult GetAllByShop([FromQuery] int ShopID)
        {
            try
            {

                var shop = _iShopService.GetinforById(ShopID);

                if (shop != null )
                {
                    return Ok(shop);
                }
                else
                {
                    return NotFound("No products found for this shop.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("History-Sold")]
        public IActionResult ProductSold(int shopId)
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
                   
                        var list = _iShopService.GetProductSold(shopId, userID);
                      
                        return Ok(list);
                  
                }
                return BadRequest(new { message = "Invalid UserId." });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
    }

}

