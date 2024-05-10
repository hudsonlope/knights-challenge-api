using System;
using System.Globalization;
using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using MongoDB.Driver;
using Knights.Challenge.Domain.Interfaces.Repository;
using Knights.Challenge.Domain.Entities;
using Knights.Challenge.Data.Repositories;
using Knights.Challenge.Domain.Interfaces.Cache;
using Knights.Challenge.Domain.Configuration;
using Knights.Challenge.Domain.Interfaces.Service;
using Knights.Challenge.Service.Services;
using Knights.Challenge.Domain.DTOs.Request;

namespace Knights.Challenge.Api
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Database
            var mongoDBSettings = _configuration.GetSection(Constants.MongoDBSettings).Get<MongoDBSettings>();

            services.AddSingleton<IMongoClient>(ServiceProvider => new MongoClient(mongoDBSettings.ConnectionString));
            services.AddScoped<IMongoDatabase>(ServiceProvider =>
            {
                var mongoClient = ServiceProvider.GetService<IMongoClient>();
                return mongoClient.GetDatabase(mongoDBSettings.DatabaseName);
            });
            #endregion Database

            #region Redis
            services.AddStackExchangeRedisCache(o =>
            {
                var redisSettings = _configuration.GetSection(Constants.RedisSettings).Get<RedisSettings>();
                o.InstanceName = redisSettings.InstanceName;
                o.Configuration = redisSettings.ConnectionString;
            });

            services.AddScoped<IRedisService, RedisService>();
            #endregion Redis

            #region Repositorys
            services.AddScoped<IBaseRepository<BaseEntity>, BaseRepository<BaseEntity>>();
            services.AddScoped<IKnightRepository, KnightRepository>();
            #endregion Repository

            #region Services
            services.AddScoped<IKnightService, KnightService>();
            #endregion Services

            #region AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                //Get all profiles
                mc.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            });

            services.AddSingleton(mappingConfig.CreateMapper());
            #endregion AutoMapper

            #region Fluent Validation DTO's
            services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters()
                    //.AddFluentValidation(config =>
                    //{
                    //    config.ValidatorOptions.LanguageManager.Culture = new CultureInfo("pt-br");
                    //})
                    .AddMvc()
                    .AddNewtonsoftJson(option =>
                    {
                        option.SerializerSettings.Converters.Add(new IsoDateTimeConverter
                        {
                            DateTimeStyles = DateTimeStyles.AdjustToUniversal,
                            DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ"
                        }); ;
                    });

            services.AddTransient<IValidator<KnightRequestDTO>, KnightRequestDTOValidator>();
            services.AddTransient<IValidator<NewNicknameRequestDTO>, NewNicknameRequestDTOValidator>();
            #endregion

            services.AddControllers()
                    .AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddSwaggerGen(x =>
            {
                x.EnableAnnotations();
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Knights.Challenge.Api",
                    Description = "CRUD de Heróis",
                    Contact = new OpenApiContact
                    {
                        Name = "Teste",
                        Email = "admin@teste.com.br",
                        Url = new Uri("http://www.teste.com.br/"),
                    }

                });
            });

            ConfigureCors(services);

        }

        private static void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins(new string[] { "*.heroku.com.br" })
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .AllowAnyHeader();
                });

                option.AddPolicy("LocalCorsPolicy", builder =>
                {
                    builder.SetIsOriginAllowed((host) => true)
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .AllowAnyHeader();
                });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            app.UseCors("LocalCorsPolicy");

            //app.UseAzureAdAuthConfiguration();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Knights.Challenge.Api");
                options.RoutePrefix = "swagger";
            });

            app.UseSwaggerUI();

            //app.UseHttpsRedirection();

            //app.UseAuthorization();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

        }
    }
}
