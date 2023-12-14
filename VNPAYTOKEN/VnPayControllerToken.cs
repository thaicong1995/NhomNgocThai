
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X9;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.Dto;
using WebApi.Models;
using WebApi.Models.Enum;
using WebApi.MyDbContext;
using WebApi.TokenConfig;
using WebApi.VNPAYTOKEN;

namespace WebApi.VNpay
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnPayControllerToken : ControllerBase
    {
        private readonly VnPayServiceToken _vnPayServicetoken;
        private readonly MyDb _myDb;
        private readonly Token _token;
        public VnPayControllerToken(VnPayServiceToken vnPayService, MyDb myDb, Token token)
        {
            _vnPayServicetoken = vnPayService;
            _myDb = myDb;
            _token = token;
        }


        [HttpGet("generateToken")]
        public IActionResult GenerateToken(int userId)
        {
            try
            {
                string tokenizationUrl = _vnPayServicetoken.GenerateTokenizationUrl(userId, HttpContext);
                return Ok(new { TokenizationUrl = tokenizationUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }




    }
}
