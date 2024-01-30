using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus;

public class AzureServiceBusEventBus : IEventBus
{
    private readonly IBusControl _busControl;

    public AzureServiceBusEventBus(IBusControl busControl)
    {
        _busControl = busControl;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
    {
        await _busControl.Publish(@event);
    }

    public Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
        where TRequest : class
        where TResponse : class
    {
        throw new NotImplementedException();
    }

    public Task ScheduleSendAsync<T>(DateTime scheduledTime, T message, string queueName) where T : class
    {
        throw new NotImplementedException();
    }

    public void Subscribe<TEvent, TEventHandler>(string queueName)
        where TEvent : class
        where TEventHandler : IEventHandler<TEvent>
    {
        throw new NotImplementedException();
    }
}
