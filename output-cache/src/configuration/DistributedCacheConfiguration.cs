using System;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using output_cache.options;

namespace output_cache.configurations;

public static class DistributedCacheConfiguration
{
    public static IServiceCollection AddDistributedRedisCache(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register Redis options
        services.Configure<CacheOptions>(
            configuration.GetSection(CacheOptions.ConfigurationSection));

        services.AddStackExchangeRedisCache(options =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var redisOptions = serviceProvider.GetRequiredService<IOptions<CacheOptions>>().Value;

            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { redisOptions.ConnectionString },
                AbortOnConnectFail = false,
                AsyncTimeout = redisOptions.AsyncTimeout,
                ConnectTimeout = redisOptions.ConnectTimeout,
                ConnectRetry = redisOptions.ConnectRetry
            };

            options.ConfigurationOptions = configurationOptions;
            options.InstanceName = redisOptions.InstanceName;
        });

        services.Configure<DistributedCacheEntryOptions>(options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            options.SlidingExpiration = TimeSpan.FromMinutes(2);
        });

        services.AddOutputCache();

        return services;
    }
}