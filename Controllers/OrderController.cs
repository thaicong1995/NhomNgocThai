
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X9;
using System.Security.Claims;
using WebApi.Dto;
using WebApi.Models;
using WebApi.Models.Enum;
using WebApi.Sevice.Interface;
using WebApi.Sevice.Service;
using WebApi.TokenConfig;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _iOrderService;
        private readonly Token _token;

        public OrderController(IOrderService orderService, Token token)
        {
            _iOrderService = orderService;
            _token = token;
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateOrder([FromBody] List<int> selectedProductIds)
        {
            try
            {

                var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userClaims != null && int.TryParse(userClaims.Value, out int userId))
                {
                    var tokenStatus = _token.CheckTokenStatus(userId);

                    if (tokenStatus == StatusToken.Expired)
                    {
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }

                    if (selectedProductIds.Count == 0)
                    {
                        return BadRequest("No selected items in the cart.");
                    }

                    var createdOrders = _iOrderService.CreateOrders(selectedProductIds, userId); // Truyền danh sách ID sản phẩm và userId

                    return Ok(createdOrders);
                }
                else
                {
                    return BadRequest("Invalid UserId.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpPost("pay")]
        public ActionResult<Order> Pay([FromQuery]string orderNo, [FromBody] OrderDto orderDto)
        {
            try
            {
                var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userClaims != null && int.TryParse(userClaims.Value, out int userID))
                {
                    var tokenStatus = _token.CheckTokenStatus(userID);

                    if (tokenStatus == StatusToken.Expired)
                    {
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }
     
                   List< Order> order = _iOrderService.PayOrder(orderNo, orderDto, userID);

                    return Ok(order);
                }
                else
                {
                    return BadRequest(new { message = "Invalid UserId." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }


        [Authorize]
        [HttpPost("refund")]
        public ActionResult<List<Order>> RefundProducts([FromQuery] string orderNo, [FromQuery] int ProductId)
        {
            try
            {
                var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                if (userClaims != null && int.TryParse(userClaims.Value, out int userId))
                {
                    var tokenStatus = _token.CheckTokenStatus(userId);

                    if (tokenStatus == StatusToken.Expired)
                    {
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }

                    // Gọi phương thức RefundProducts từ OrderService để xử lý hoàn trả một số lượng sản phẩm
                    var refundedOrders = _iOrderService.RefundProduct(orderNo, userId, ProductId);

                    return Ok(refundedOrders);
                }
                else
                {
                    // Invalid or missing userID in the token
                    return BadRequest(new { message = "Invalid UserId." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }


        [Authorize]
        [HttpGet("history-buy")]
        public ActionResult<List<Order>> HistoryBuy()
        {
            try
            {
                var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                if (userClaims != null && int.TryParse(userClaims.Value, out int userId))
                {
                    var tokenStatus = _token.CheckTokenStatus(userId);

                    if (tokenStatus == StatusToken.Expired)
                    {
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }

                    // Gọi phương thức RefundProducts từ OrderService để xử lý hoàn trả một số lượng sản phẩm
                    var history = _iOrderService.HistoryBuy(userId);

                    return Ok(history);
                }
                else
                {
                    // Invalid or missing userID in the token
                    return BadRequest(new { message = "Invalid UserId." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
    }
}
