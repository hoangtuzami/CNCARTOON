using CNCARTOON.Utility.Constants;
using StackExchange.Redis;

namespace CNCARTOON.API.Extentions
{
    public static class RedisServiceExtentions
    {
        public static WebApplicationBuilder AddRedisCache(this WebApplicationBuilder builder)
        {
            string redisConnectionString = builder.Configuration.GetSection("Redis")[StaticConnectionString.REDIS_ConnectionString];
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(redisConnectionString, true);
                configuration.AbortOnConnectFail = false;
                return ConnectionMultiplexer.Connect(configuration);
            });

            return builder;
        }
    }
}
