using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestProject.Models;
using TestProject.Services;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace TestProject
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
            string connection =
                Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DiaryDBContext>(options => options.UseSqlServer(connection));

            // аутентификация
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            // добавление кэширования
            services.AddMemoryCache();

            // внедрение зависимости ClassService
            services.AddTransient<ClassService>();
            // внедрение зависимости StudentService
            services.AddTransient<StudentService>();
            // внедрение зависимости SubjectService
            services.AddTransient<SubjectService>();

            // внедрение зависимости HomeService для unit-теста
            services.AddTransient<IHomeService, HomeService>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            GlobalDiagnosticsContext.Set("connectionString", Configuration.GetConnectionString("DefaultConnection"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "students",
                    pattern: "{controller=Students}/{action=Index}/{name?}");

                endpoints.MapControllerRoute(
                    name: "subjectgrade",
                    pattern: "{controller=SubjectGrade}/{action=Grade}/{id?}");

                endpoints.MapControllerRoute(
                    name: "classsubject",
                    pattern: "{controller=ClassSubject}/{action=Index}/{name?}");
            });
        }
    }
}
