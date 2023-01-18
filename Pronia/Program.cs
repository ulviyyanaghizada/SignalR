using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pronia.Abstractions.Services;
using Pronia.DAL;
using Pronia.Hubs;
using Pronia.Models;
using Pronia.Service;

namespace Pronia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Ulviyya"));
            });
            builder.Services.AddScoped<LayoutService>();
            builder.Services.AddSession(opt =>
            {
                opt.IdleTimeout=TimeSpan.FromSeconds(10);
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 6;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
                opt.Lockout.AllowedForNewUsers = true;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
            builder.Services.AddScoped<IEmailService, EmailService>();
			builder.Services.AddSignalR();

			builder.Services.AddHttpContextAccessor();
			var app = builder.Build();

            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
			app.MapHub<ChatHub>("/chatHub");
			app.Run();
        }
    }
}