using StackExchange.Redis;

namespace OnlineShop_Api.Extensions;
public static class RedisRegistration
{
    public static ConnectionMultiplexer ConfigureRedis(this IServiceProvider services, IConfiguration configuration)
    {
        var redisConf = ConfigurationOptions.Parse(configuration["RedisSettings:ConnectionString"], true);
        redisConf.ResolveDns = true;

        return ConnectionMultiplexer.Connect(redisConf);
    }

}
