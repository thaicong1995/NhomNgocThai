using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using WebApi.Sevice.Service;
using WebApi.TokenConfig;
using WebApi.Models;
using WebApi.Sevice.Interface;
using WebApi.Dto;

[Route("api")]
[ApiController]
public class DecodeController : ControllerBase
{
    
    private readonly Token _token;

    public DecodeController( Token token)
    {
        
        _token = token;
    }


    [Authorize]
    [HttpGet("decode")]
    public ActionResult<WalletDto> DecodeToken()
    {
        try
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var authorizationHeader = HttpContext.Request.Headers["Authorization"];
            var token = authorizationHeader.ToString().Replace("Bearer ", "");

            var principal = _token.DecodeToken(token);

            if (principal == null)
            {
                return BadRequest("Invalid token");
            }

            var userWithWallet = _token.GetUserWithWallet(principal);

            var nameClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            if (nameClaim == null)
            {
                return BadRequest("User information not full claim");
            }

            string userName = nameClaim.Value;

            if (userId != int.Parse(userIdClaim.Value))
            {
                return Unauthorized("Fail token decode. (#user)");
            }

            return Ok(userWithWallet);
        }
        catch (Exception ex)
        {

            return BadRequest("Error decoding token");
        }
    }
}
    




