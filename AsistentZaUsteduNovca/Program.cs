using AsistentZaUsteduNovca.Models;
using Microsoft.EntityFrameworkCore;

namespace AsistentZaUsteduNovca
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //builder je objekat koji dodaje sve servise? To je vrv ovo .Services

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // DEPENDENCY INJECTION
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            // prosledili smo i vrednosti za parametre konstruktora
            options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"))); // ovde treba connection string baze (on se nalazi u appsettings.json)
            // pozivom ove funkcije govorimo programu da cemo koristii sql server DB


            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXledHRXRmNYU0BzXkY=");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}");
                // ovo ce biti difoltna akcija pri pokretanju znaci prebacice nas na homepage

            app.Run();
        }
    }
}
