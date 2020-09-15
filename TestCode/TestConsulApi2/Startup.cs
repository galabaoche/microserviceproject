using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestConsulApi2
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            lifetime.ApplicationStarted.Register(OnStarted);
            lifetime.ApplicationStopped.Register(OnStopped);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void OnStopped()
        {
            var (client, agentReg) = GetAgentServiceRegistration();
            client.Agent.ServiceDeregister(agentReg.ID);
        }
        private void OnStarted()
        {
            var (client, agentReg) = GetAgentServiceRegistration();
            client.Agent.ServiceRegister(agentReg).Wait();
        }

        private (ConsulClient, AgentServiceRegistration) GetAgentServiceRegistration()
        {
            var client = new ConsulClient(); //use default host:port which is localhost:8500
            var httpCheck = new AgentServiceCheck
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                Interval = TimeSpan.FromSeconds(10),
                HTTP = "http://localhost:3722/healthcheck"
            };
            var agentReg = new AgentServiceRegistration
            {
                Check = httpCheck,
                Address = "localhost",
                Name = "TestConsulApi",
                ID = "TestConsulApi:3722",
                Port = 3722,
                Tags = new string[] { "test_api" }

            };
            return (client, agentReg);
        }
    }
}
