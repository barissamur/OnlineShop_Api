using Microsoft.AspNetCore.SignalR;

namespace Shop.Web.SignalR;

public class StockHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}
