﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data;
using Lok.Data.Interface;
using Lok.Data.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lok
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddScoped<IMangoContext, MongoContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<ISubService, SubServiceRepository>();
            services.AddScoped<IEthinicalGroup, EthinicalGroupRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<ISubGroupRepository, SubGroupRepository>();
            services.AddScoped<ICategoryInterface, ICategoryRepository>();

            services.AddScoped<IAdvertisiment, AdvertisimentRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ILoginInterface, LoginRepository>();
            services.AddScoped<IRoleInterface, RoleRepository>();
            services.AddScoped<IAuthinterface, AuthRepository>();
            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
            });
            services.AddAuthentication("Cookie")
                .AddCookie("Cookie",
                    options =>
                    {
                        options.LoginPath = new PathString("/Account/Login/");
                        options.AccessDeniedPath = new PathString("/Account/Forbidden/");
                    });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",
                    policy => policy.RequireRole("Admin"));

                //options.AddPolicy(,
                //    policy =>
                //    {
                //        policy.RequireAuthenticatedUser();
                //        policy.RequireRole("Administrator");
                //        policy.Requirements.Add(new AlbumOwnerRequirement());
                //    }
                //);

            });

        }

        //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //   .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        //       options =>
        //       {
        //           options.LoginPath = new PathString("/Account/Login/");
        //           options.AccessDeniedPath = new PathString("/Account/Forbidden/");
        //       });


        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();

            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
