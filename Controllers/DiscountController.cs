using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Models;
using WebApi.MyDbContext;
using WebApi.Sevice.Interface;
using WebApi.Sevice.Service;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _iDiscountService;
        public DiscountController(IDiscountService discountService)
        {
            _iDiscountService = discountService;
        }

        [HttpPost("CreateDiscount")]
        public IActionResult CreateDiscount([FromBody] DiscountDTo discount)
        {
            try
            {
                if (discount == null)
                {
                    return BadRequest("Discount information is missing.");
                }

                _iDiscountService.CreateDiscount(discount);

                return Ok("Discount created successfully");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }


        [HttpPost("GetDiscount")]
        public IActionResult GetAllDiscount()
        {
            try
            {
                var list = _iDiscountService.GetAllDiscount();
                return Ok(list);

            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }
    }
}
