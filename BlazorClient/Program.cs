using System.Threading.Tasks;
using BlazorClient.WebAssemblyHostConfiguration;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args)
                .ConfigureWebAssemblyHost(options => options.UseStartup<Startup>());

            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }
    }
}
