using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppsOnSF.Common.Options;
using MassTransit;
using MassTransit.AspNetCoreIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmsService.EventBus;

namespace SmsService
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
            services.AddHealthChecks();

            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register MassTransit
            services.AddMassTransit(CreateBus, cfg =>
            {
                cfg.AddConsumer<SendMobileCodeCommandConsumer>();
            });
        }

        private IBusControl CreateBus(IServiceProvider provider)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var option = Configuration.GetSection("RabbitMQ").Get<RabbitMQOption>();

                var host = cfg.Host(new Uri(option.HostAddress), h =>
                {
                    h.Username(option.Username);
                    h.Password(option.Password);
                });

                cfg.ReceiveEndpoint("Base.Csi.SmsService", c =>
                {
                    c.ConfigureConsumer<SendMobileCodeCommandConsumer>(provider);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHealthChecks("/health");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
