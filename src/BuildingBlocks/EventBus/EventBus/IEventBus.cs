using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus;

public interface IEventBus
{
    // Olay veya mesaj yayınlama
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;

    // Belirli bir türdeki olayları işlemek için abone olma
    void Subscribe<TEvent, TEventHandler>(string queueName)
        where TEvent : class
        where TEventHandler : IEventHandler<TEvent>;

    // İstek gönderme ve yanıt alma
    Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
        where TRequest : class
        where TResponse : class;

    // Planlanmış mesaj gönderme
    Task ScheduleSendAsync<T>(DateTime scheduledTime, T message, string queueName) where T : class;

    // Diğer gerekli metodlar burada eklenebilir.
}

public interface IEventHandler<in TEvent> where TEvent : class
{
    Task Handle(TEvent @event);
}