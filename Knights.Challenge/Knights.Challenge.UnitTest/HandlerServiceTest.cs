using Knights.Challenge.Data.Repositories;
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
                      .AddTransient<IBaseRepository<BaseEntity>, BaseRepository<BaseEntity>>()
                      //.AddDependencySharedPackageDatabase(config)
                      //.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
                      //.AddSingleton<IFinancialTransactionService, FinancialTransactionService>()
                      //.AddSingleton<IFinancialTransactionPRepository, FinancialTransactionPRepository>()
                      //.AddSingleton<IFinancialTransactionRRepository, FinancialTransactionRRepository>()
                      //.AddSingleton<IBasePersistenceRepository<BaseEntity>, BasePersistenceRepository<BaseEntity>>()
                      //.AddScoped(typeof(IBasePersistenceRepository<>), typeof(BasePersistenceRepository<>))
                      .AddScoped<IKnightService, KnightService>()
                      .AddScoped<IRedisService, RedisService>()
                      //.AddScoped<IFinancialTransactionPRepository, FinancialTransactionPRepository>()
                      .AddLogging()
                      .AddAutoMapper(Assembly.GetEntryAssembly())
                      //.AddDbContext<DailyCashFlowContext>(options => options.UseSqlServer(config.GetSection("ConnectionString").Value))
                      .BuildServiceProvider();
        }

        //private IConfiguration GetConfiguration()
        //{
        //    return new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .AddEnvironmentVariables()
        //        .Build();
        //}
    }
}
