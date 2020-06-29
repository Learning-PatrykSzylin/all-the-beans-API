using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllTheBeans.Core.Interfaces;
using AllTheBeans.Infrastructure.BeanAdvert;
using AllTheBeans.Infrastructure.Beans;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace AllTheBeans.API
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
            services.AddControllers();
            services.AddCors();

            // Register swagger generator
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Beans of the Day API",
                    Description = "API responsible for storing beans for your business. Additionally, it also allows you to create bean adverts on a specific date.",
                    TermsOfService = new Uri("https://example.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "Patryk Szylin",
                        Email = "patryk.szylin.dev@gmail.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Usage upon request",
                        Url = new Uri("https://example.com")
                    }
                });
            });

            // Configure for DI
            services.AddScoped<IBeanRepository, BeanRepositryPgsql>();
            services.AddScoped<IBeanAdvertRepository, BeanAdvertRepositoryPgsql>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // Instruct the app to use swagger
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Beans of the Day API");
                s.RoutePrefix = string.Empty; // This will result in accessing swagger docs at the root of the url -> "/"
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
