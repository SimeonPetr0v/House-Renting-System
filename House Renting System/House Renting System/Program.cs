using House_Renting_System.Middlewares;
using House_Renting_System.Services.Contracts;
using House_Renting_System.Services.Implementations;
using HouseRentingSystem.Data.Data;
using HouseRentingSystem.Data.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace House_Renting_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<HouseRentingDbContext>(opt => opt.UseSqlServer(connectionString));



            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequiredLength = 6;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireUppercase = false;
                    opt.SignIn.RequireConfirmedEmail = false;
                }
            )
                .AddEntityFrameworkStores<HouseRentingDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/User/AccessDenied";
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IHouseService, HouseService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Home/StatusCodeHandler", "?statusCode={0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.Use(async (context, next) =>
            {
                var path = context.Request.Path;
                Console.WriteLine(path);
                await next();

                var statusCode = context.Response.StatusCode;
                Console.WriteLine(statusCode);
            });
            app.UseCustom();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}