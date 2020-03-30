using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data;
using Lok.Data.Interface;
using Lok.Data.Repository;
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
            services.AddScoped<IReligionRepository, ReligionRepository>();
            services.AddScoped<IEmploymentRepository, EmploymentRepository>();
            services.AddScoped<IEducationLevelRepository, EducationLevelRepository>();
            services.AddScoped<IVargaRepository, VargaRepository>();
            services.AddScoped<IBoardNameRepository, BoardNameRepository>();
            services.AddScoped<IFacultyRepository, FacultyRepository>();
            services.AddScoped<IVargaRepository, VargaRepository>();
            services.AddScoped<ISewaRepository, SewaRepository>();
            services.AddScoped<IShreniTahaRepository, ShreniTahaRepository>();
            services.AddScoped<IAwasthaRepository, AwasthaRepository>();
            services.AddScoped<IApplicantRepository, ApplicantRepository>();
            services.AddScoped<IOccupationRepository, OccupationRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

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
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
