using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TodoApiMicrosoft.Models;

namespace TodoApiMicrosoft
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
            //* Connect String
            services.AddControllers();
            services.AddDbContext<TodoContext>(opt =>
            opt.UseInMemoryDatabase("TodoList"));

            // swagger
            services.AddSwaggerGen(options =>
                    {
                        options.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Version = "v1",
                            Title = "ToDo API",
                            Description = "An ASP.NET Core Web API for managing ToDo items",
                            TermsOfService = new Uri("https://example.com/terms"),
                            Contact = new OpenApiContact
                            {
                                Name = "Example Contact",
                                Url = new Uri("https://example.com/contact")
                            },
                            License = new OpenApiLicense
                            {
                                Name = "Example License",
                                Url = new Uri("https://example.com/license")
                            }
                        });
                        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});
                // The UseSwaggerUI method call enables the Static File Middleware.
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    // To serve the Swagger UI at the app's root (https://localhost:<port>/), set the RoutePrefix property to an empty string
                    options.RoutePrefix = string.Empty;
                    options.InjectStylesheet("/swagger-ui/custom.css");
                });
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

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
