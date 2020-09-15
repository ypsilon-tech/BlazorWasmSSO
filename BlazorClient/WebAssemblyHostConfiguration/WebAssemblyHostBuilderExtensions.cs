using System;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorClient.WebAssemblyHostConfiguration
{
    public static class WebAssemblyHostBuilderExtensions
    {
        public static WebAssemblyHostBuilder ConfigureWebAssemblyHost(this WebAssemblyHostBuilder builder, Action<WebAssemblyHostConfigurationBuilder.WebAssemblyHostConfiguration.WebAssemblyHostConfigurationBuilderOptions> options)
        {
            var wasmHostConfigBuilder = new WebAssemblyHostConfigurationBuilder(builder);
            wasmHostConfigBuilder
                .Build(options)
                .Run();

            return builder;
        }
    }
}