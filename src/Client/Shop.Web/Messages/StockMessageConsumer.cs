using MassTransit;
using OnlineShop_Api.Messages;

namespace Shop.Web.Messages;

public class StockMessageConsumer : IConsumer<StockMessage>
{
    private readonly ILogger<StockMessageConsumer> logger;


    public StockMessageConsumer(ILogger<StockMessageConsumer> logger)
    {
        this.logger = logger;
    }


    public async Task Consume(ConsumeContext<StockMessage> context)
    {
        logger.LogInformation("Received message: {Content}", context.Message.Content);
    }
}
