using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MESF.Core.ServiceManagement;
using MESF.Core.ServiceManagement.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using MESF.Core.ServiceManagement.Core.Interfaces;
using MESF.Core.ServiceManagement.Infrastructure.Data;

//Nswag implementation
using NJsonSchema;
using NSwag.AspNetCore;
using System.Reflection;


namespace SampleServiceManagement.AspNetCore
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
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);

            services.AddTransient<IMessaging, Messaging>();

            services.AddDbContext<ServiceManagementContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ServicesConnection"))
            );

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            // Register the Swagger services
            services.AddSwaggerDocument(document => document.DocumentName = "Swagger document");
            services.AddOpenApiDocument(document => document.DocumentName = "OpenQpi document");

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwagger();
            app.UseSwaggerUi3();



            app.UseMvc();
        }
    }
}
