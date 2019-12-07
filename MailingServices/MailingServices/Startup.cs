using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenServices.Models;
using MailingServices.Models;
using MailingServices.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steeltoe.Discovery.Client;
using Swashbuckle.AspNetCore.Swagger;

namespace Deverate
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
            services.AddSingleton<IHostedService>(provider => new Consumer("AccountToEmail"));
            services.AddSingleton<IHostedService>(provider => new Consumer("TestEmployeeToEmail"));
            services.AddDiscoveryClient(Configuration);
            services.AddMvc();
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            var connection = @"Server=deverate.cjd2ogwhqpge.ap-southeast-1.rds.amazonaws.com;Database=Deverate;User ID=deverate;Password=pass4deverate;Trusted_Connection=False;";
            services.AddDbContext<DeverateContext>(options => options.UseSqlServer(connection));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "My First ASP.NET Core Web API"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseDiscoveryClient();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
