using MassTransit;

namespace EventBus;

public class RabbitMQEventBus : IEventBus
{
    private readonly IBusControl _busControl;

    public RabbitMQEventBus(IBusControl busControl)
    {
        _busControl = busControl;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
    {
        await _busControl.Publish(@event);
    }

    public void Subscribe<TEvent, TEventHandler>(string queueName)
        where TEvent : class
        where TEventHandler : IEventHandler<TEvent>
    {
        _busControl.ConnectReceiveEndpoint(queueName, cfg =>
        {
            cfg.Handler<TEvent>(context =>
            {
                var handler = Activator.CreateInstance<TEventHandler>();
                return handler.Handle(context.Message);
            });
        });
    }


    public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var client = _busControl.CreateRequestClient<TRequest>();
        var response = await client.GetResponse<TResponse>(request);
        return response.Message;
    }


    public async Task ScheduleSendAsync<T>(DateTime scheduledTime, T message, string queueName) where T : class
    {
        var delay = scheduledTime - DateTime.UtcNow; // Şu an ile planlanan zaman arasındaki gecikme
        if (delay < TimeSpan.Zero)
        {
            delay = TimeSpan.Zero; // Geçmiş bir zaman için gecikmeyi sıfırla
        }

        var endpointUri = new Uri($"queue:{queueName}");
        var scheduler = _busControl.CreateMessageScheduler();

        await scheduler.ScheduleSend(endpointUri, DateTime.UtcNow + delay, message);
    }
}
