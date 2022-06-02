using NLog;
using NLog.Web;

namespace RecursivaChallenge
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

            logger.Debug("Inicio de Aplicacion.");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                var startUp = new Startup(builder.Configuration);

                startUp.ConfigureBuilder(builder);

                startUp.ConfigureServices(builder.Services);

                var app = builder.Build();

                startUp.Configure(app, app.Environment);

                app.Run();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Surgio una excepcion sin manejar");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}