using Microsoft.EntityFrameworkCore;
using RecursivaChallenge.Data;
using RecursivaChallenge.Repository;
using RecursivaChallenge.Repository.Contract;
using NLog.Extensions.Logging;
using NLog.Web;


namespace RecursivaChallenge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions => sqlServerOptions.CommandTimeout(60)));

            services.AddScoped<ISociosRepository, SociosRepository>();

            services.AddMvc();
        }

        public void ConfigureBuilder(WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllerRoute(
                name: "default",
                pattern: "{controller=Socio}/{action=Index}/{id?}");

            });
        }
    }
}
