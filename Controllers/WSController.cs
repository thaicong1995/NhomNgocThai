
// WSController.cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WebApi.Dto;
using WebApi.Models;
using WebApi.Sevice.Interface;
using WebApi.TokenConfig;

namespace WebApi.Controllers
{
    [Route("api/ws")]
    [ApiController]
    public class WsController : ControllerBase
    {
        private readonly IProductService _iProductService;

        public WsController(IProductService productService, Token token)
        {
            _iProductService = productService;
        }
        [HttpGet("wss/page={page}")]
        public async Task Get(int page = 1)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                // Generate random values for X and Y every 2 seconds
                var random = new Random();
                while (webSocket.State == WebSocketState.Open)
                {
                    List<Product> products = _iProductService.GetAll(page);
                    string jsonString = JsonSerializer.Serialize(products);
                    var buffer = Encoding.UTF8.GetBytes(jsonString);
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(buffer),
                        WebSocketMessageType.Text, true, CancellationToken.None);

                    await Task.Delay(2000);
                }

                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    "Connection closed by the server", CancellationToken.None);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
