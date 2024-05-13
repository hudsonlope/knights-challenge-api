using Knights.Challenge.Data.Repositories;
using Knights.Challenge.Domain.Configuration;
using Knights.Challenge.Domain.Entities;
using Knights.Challenge.Domain.Interfaces.Cache;
using Knights.Challenge.Domain.Interfaces.Repository;
using Knights.Challenge.Domain.Interfaces.Service;
using Knights.Challenge.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Knights.Challenge.UnitTest
{
    public class HandlerServiceTest
    {
        public IServiceProvider ServiceProviderMock { get; set; }

        public HandlerServiceTest() : base()
        {
            var config = new ConfigurationBuilder()
                   .SetBasePath(AppContext.BaseDirectory)
                   .AddJsonFile("appsettings.json", optional: false)
                   .AddEnvironmentVariables()
                   .Build();

            ServiceProviderMock = new ServiceCollection()
                      .AddSingleton<IConfiguration>(config)
                      .AddScoped<IBaseRepository<BaseEntity>, BaseRepository<BaseEntity>>()
                      .AddScoped<IKnightRepository, KnightRepository>()
                      .AddScoped<IKnightService, KnightService>()
                      .AddLogging()
                      .AddAutoMapper(Assembly.GetEntryAssembly())
                      .AddStackExchangeRedisCache(o =>
                      {
                          var redisSettings = config.GetSection(Constants.RedisSettings).Get<RedisSettings>();
                          o.InstanceName = redisSettings.InstanceName;
                          o.Configuration = redisSettings.ConnectionString;
                      })
                      .AddScoped<IRedisService, RedisService>()
                      .BuildServiceProvider();
        }

    }
}
