using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class WS : ControllerBase
    {
        [HttpGet("ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                //sinh ngau nhien 2 gia tri x, y thau doi 2 s
                var random = new Random();
                while(webSocket.State == WebSocketState.Open)
                {
                    int x = random.Next(1, 100);
                    int y = random.Next(1, 100);
                    var buffer = Encoding.UTF8.GetBytes($"{{\"X\":{x}, \"Y\":{y}}}");
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(buffer),
                        WebSocketMessageType.Text,true, CancellationToken.None);
                    await Task.Delay(2000);
                }
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    "Connect closef by the server", CancellationToken.None);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
