using MassTransit;
using Microsoft.AspNetCore.SignalR;
using OnlineShop_Api.Messages;
using Shop.Web.SignalR;

namespace Shop.Web.Messages;

public class StockMessageConsumer : IConsumer<StockMessage>
{
    private readonly ILogger<StockMessageConsumer> logger;
    private readonly IHubContext<StockHub> hubContext;

    public StockMessageConsumer(ILogger<StockMessageConsumer> logger,
        IHubContext<StockHub> hubContext)
    {
        this.logger = logger;
        this.hubContext = hubContext;
    }


    public async Task Consume(ConsumeContext<StockMessage> context)
    {
        await hubContext.Clients.All.SendAsync("ReceiveMessage", context.Message.Content);
        logger.LogInformation("Received message: {Content}", context.Message.Content);
    }
}
