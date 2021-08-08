using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CordovaMudBlazor.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHostBuilder builder = CreateHostBuilder(args);
            IHost host = builder.Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(args);
            builder = builder.ConfigureAppConfiguration((builderContext, config) =>
            {
                //IHostEnvironment env = builderContext.HostingEnvironment;
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);                
            });
            builder = builder.ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(x => x.Limits.MaxResponseBufferSize = null);
                    webBuilder.UseStartup<Startup>();

                }
            );

            return builder;
        }
    }
}
