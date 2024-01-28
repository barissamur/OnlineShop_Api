using MassTransit;

namespace OnlineShop_Api.Messages;

public class TestMessageConsumer : IConsumer<TestMessage>
{
    private readonly ILogger<TestMessageConsumer> logger;


    public TestMessageConsumer(ILogger<TestMessageConsumer> logger)
    {
        this.logger = logger;
    }


    public async Task Consume(ConsumeContext<TestMessage> context)
    {
        logger.LogInformation("Received message: {Content}", context.Message.Content);
         
    }
}
