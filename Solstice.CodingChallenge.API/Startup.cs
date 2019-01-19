using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Solstice.CodingChallenge.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Solstice.CodingChallenge.Provider;
using AutoMapper;
using FluentValidation;
using Solstice.CodingChallenge.API.Dtos.Requests;
using Solstice.CodingChallenge.Domain.Data.Seed;
using Solstice.CodingChallenge.API.Controllers.Middleware;

namespace Solstice.CodingChallenge.API
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IDatabaseProvider, DatabaseProvider>();

            services.AddAutoMapper();

            services.AddMvc();

            services.AddTransient<IValidator<EmailsBaseClass>, ContactPhoneValidator>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Contact Managment API", Version = "v1", Description = "View, create, edit and delete contacts." });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware(typeof(IOMiddleware));

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>().EnsureSeeded();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Contact Managment API");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
