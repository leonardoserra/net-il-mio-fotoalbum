using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;
using net_il_mio_fotoalbum.Database;
using NuGet.ContentModel;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using net_il_mio_fotoalbum.Areas.Identity.Data;
using System.Text.Json.Serialization;

namespace net_il_mio_fotoalbum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<PhotoAlbumsContext>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PhotoAlbumsContext>();
            builder.Services.AddScoped<PhotoAlbumsContext, PhotoAlbumsContext>();
            builder.Services.AddControllersWithViews();

            //setting JSON directive so doesn't try to serialize the Cycliyng informations
            builder.Services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Photo/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            //dotnet-aspnet-codegenerator identity --dbContext ProfileContext --files "Account.Login;Account.Logout;Account.Register" -tfm "net60"
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Photo}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}