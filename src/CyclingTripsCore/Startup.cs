using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CyclingTripsCore.Services;
using CyclingTrips.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AutoMapper;
using CyclingTrips.ViewModels;

namespace CyclingTripsCore
{
    public class Startup
    {
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _config = builder.Build();
        }

        private IConfigurationRoot _config;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);

            
            //if(_env.IsEnvironment("Development") || _env.IsEnvironment("Testing"))
            //{
            //    services.AddScoped<IMailService, DebugMailService>();
            //}
            //else
            //{
            //    // Implement a real mail service
            //}

            services.AddDbContext<CyclingTripsDbContext>();

            services.AddScoped<ICyclingTripsRepository, CyclingTripsRepository>();

            services.AddTransient<CyclingTripsContextSeedData>();

            services.Configure<IISOptions>(options =>
            {
                options.ForwardWindowsAuthentication = true;
            });

            services.AddIdentity<CyclingTripsUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Account/Login";
            })
            .AddEntityFrameworkStores<CyclingTripsDbContext>();

            // Add framework services.
            services.AddMvc();

            services.AddCors();            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, CyclingTripsContextSeedData seeder)
        {
            loggerFactory.AddConsole(_config.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug(LogLevel.Information);
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors(builder =>
            builder.AllowAnyOrigin()
            .WithHeaders("*")
            );


            app.UseStaticFiles();

            app.UseIdentity();

            Mapper.Initialize(config =>
            {
                config.CreateMap<Trip, TripViewModel>().ReverseMap();
                config.CreateMap<Stop, StopViewModel>().ReverseMap();
                config.CreateMap<Comment, CommentViewModel>().ReverseMap();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            seeder.EnsureSeedData().Wait();
        }
    }
}
