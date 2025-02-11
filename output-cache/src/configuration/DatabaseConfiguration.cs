using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using output_cache.data;
using output_cache.options;

namespace output_cache.configuration;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register database options
        services.Configure<DatabaseOptions>(
            configuration.GetSection(DatabaseOptions.ConfigurationSection));

        // Register DbContext
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            
            options.UseNpgsql(dbOptions.ConnectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: dbOptions.MaxRetryCount,
                    maxRetryDelay: TimeSpan.FromSeconds(dbOptions.MaxRetryDelaySeconds),
                    errorCodesToAdd: null);
                sqlOptions.CommandTimeout(dbOptions.CommandTimeout);
            });
            
            if (dbOptions.EnableDetailedErrors)
                options.EnableDetailedErrors();
                
            if (dbOptions.EnableSensitiveDataLogging)
                options.EnableSensitiveDataLogging();
        });

        return services;
    }
} 