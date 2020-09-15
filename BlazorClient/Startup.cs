using System.Net.Http;
using BlazorClient.Security;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorClient
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebAssemblyHostEnvironment HostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebAssemblyHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("api")
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { "https://localhost:5003" },
                            scopes: new[] { "weatherapi" });
                    return handler;
                });
            services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("api"));

            services.AddOidcAuthentication(options =>
                {
                    Configuration.Bind("oidc", options.ProviderOptions);
                    options.UserOptions.RoleClaim = "role";
                })
                .AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();
        }
    }
}