using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Consul;
using DotNetCore.CAP.Dashboard.NodeDiscovery;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Project.API.Application.Queries;
using Project.API.Application.Service;
using Project.API.Dtos;
using Project.Domain.AggregatesModel;
using Project.Infrastructure;
using Project.Infrastructure.Repositories;
using MSLogging = Microsoft.Extensions.Logging;

namespace Project.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));
            services.AddSwaggerGen();
            //dotnet ef migrations add init --project "Project.Infrastructure" --startup-project "Project.API" -v
            services.AddDbContext<ProjectContext>(options =>
           {
               options.UseMySQL(Configuration.GetConnectionString("MysqlProject"), sqlOptions =>
               {
                   //sqlOptions.MigrationsAssembly(typeof(ProjectContext).GetTypeInfo().Assembly.GetName().Name);
               });
               options.UseLoggerFactory(LoggerFactory.Create(builder =>
               {
                   builder.AddFilter((catelog, level) =>
                      catelog == DbLoggerCategory.Database.Command.Name && level == MSLogging.LogLevel.Information
                   ).AddConsole();
               }));
           });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "http://localhost:10000";

                options.Audience = "project_api";
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;

                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));

            services.AddCap(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MysqlEvent"))
                    .UseRabbitMQ("localhost")
                    .UseDashboard();

                // 注册节点到 Consul
                options.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 5003;
                    d.NodeId = "3";
                    d.NodeName = "CAP No.3 Node Project.API";
                });
            });

            services.AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<IProjectQueries, ProjectQueries>()
                .AddScoped<IRecommendService, TestRecommendService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
           IHostApplicationLifetime lifetime,
           IOptions<ServiceDiscoveryOptions> serviceOptions,
           IConsulClient consul)
        {
            IdentityModelEventSource.ShowPII = true;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            lifetime.ApplicationStarted.Register(()
                         => RegisterService(app, serviceOptions, consul));
            lifetime.ApplicationStopped.Register(()
                => DeRegisterService(app, serviceOptions, consul));
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterService(
        IApplicationBuilder app,
        IOptions<ServiceDiscoveryOptions> serviceOptions,
        IConsulClient consul)
        {
            var addresses = GetAddresses(app);

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(address, "HealthCheck").OriginalString
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = address.Port
                };

                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
            }
        }
        private void DeRegisterService(IApplicationBuilder app,
        IOptions<ServiceDiscoveryOptions> serviceOptions,
        IConsulClient consul)
        {
            var addresses = GetAddresses(app);
            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";
                consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            }
        }

        private List<Uri> GetAddresses(IApplicationBuilder app)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p.Replace("[::]", "127.0.0.1")));
            return addresses.ToList();
        }
    }
}
