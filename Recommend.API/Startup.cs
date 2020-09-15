using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using DnsClient;
using DotNetCore.CAP.Dashboard.NodeDiscovery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Recommend.API.Data;
using Recommend.API.Dtos;
using Recommend.API.InetgrationEventsHandler;
using Recommend.API.Policies;
using Recommend.API.Services;
using MSLogging = Microsoft.Extensions.Logging;

namespace Recommend.API
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
            services.AddHttpClient<IUserService, UserService>()
                .AddPolicyHandler(DefaultPolicy.GetRetryPolicy(5))
                .AddPolicyHandler(DefaultPolicy.GetCircuitBreakerPolicy(5));
            services.AddHttpClient<IContactService, ContactService>()
                .AddPolicyHandler(DefaultPolicy.GetRetryPolicy(5))
                .AddPolicyHandler(DefaultPolicy.GetCircuitBreakerPolicy(5));

            services.AddScoped<ProjectCreatedIntegrationEventHandler>();
            // services.AddScoped<IContactService, ContactService>()
            // .AddScoped<IUserService, UserService>();

            services.AddDbContext<RecommendDbContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("MysqlRecommend"));
                options.UseLoggerFactory(LoggerFactory.Create(builder =>
                {
                    builder.AddFilter((catelog, level) =>
                    catelog == DbLoggerCategory.Database.Command.Name && level == MSLogging.LogLevel.Information
                    ).AddConsole();
                }));
            });
            services.AddControllers();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "http://localhost:10000";

                options.Audience = "recommend_api";
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                return new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
            });
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
                options.UseMySql(Configuration.GetConnectionString("MysqlEvent"));
                //options.UseRabbitMQ(Configuration.GetConnectionString("RabbitMq"));
                options.UseRabbitMQ("localhost");
                options.UseDashboard();
                options.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 5004;
                    d.NodeId = "4";
                    d.NodeName = "CAP No.4 Node Recommend.API";
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
