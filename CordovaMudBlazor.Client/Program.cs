using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using MudBlazor.Services;
using CordovaMudBlazor.Library;

namespace CordovaMudBlazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var cult = new System.Globalization.CultureInfo("en-GB");
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cult;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cult;

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            string addr = builder.HostEnvironment.BaseAddress;

            MyHttp hp = new MyHttp();
            hp.BaseAddress = MyHttp.MyBaseAddress;
            builder.Services.AddTransient(sp => hp);
            builder.Services.AddSingleton<Callbacker>();
            builder.Services.AddMudServices();
            
            var build = builder.Build();

            IJSRuntime jrt = (IJSRuntime) build.Services.GetService(typeof(IJSRuntime));
            await build.RunAsync();
        }
    }
}
