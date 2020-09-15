using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorClient.WebAssemblyHostConfiguration
{
    public class WebAssemblyHostConfigurationBuilder
    {
        private readonly WebAssemblyHostBuilder _builder;

        public WebAssemblyHostConfigurationBuilder(WebAssemblyHostBuilder builder)
        {
            _builder = builder;
        }

        public WebAssemblyHostConfiguration Build(Action<WebAssemblyHostConfiguration.WebAssemblyHostConfigurationBuilderOptions> options)
        {
            Debug.WriteLine("Building Host Config...");
            var configBuilderOptions = new WebAssemblyHostConfiguration.WebAssemblyHostConfigurationBuilderOptions();
            options(configBuilderOptions);

            return new WebAssemblyHostConfiguration(_builder);
        }

        public class WebAssemblyHostConfiguration
        {
            private static Type _startupClassType;

            private readonly WebAssemblyHostBuilder _builder;

            public WebAssemblyHostConfiguration(WebAssemblyHostBuilder builder)
            {
                _builder = builder;
            }

            public void Run()
            {
                Debug.WriteLine("Running Host Config....");
                RunStartupIfSpecified();
            }

            private void RunStartupIfSpecified()
            {
                if (_startupClassType == null) return;

                var startupClass = CreateStartupClass(_startupClassType);
                if (startupClass == null) return;

                RunStartupConfigureServices(startupClass, _startupClassType);
            }

            private object CreateStartupClass(Type startupClassType)
            {
                Debug.WriteLine("Creating Startup class instance...");
                var constructor = startupClassType.GetConstructors().FirstOrDefault();
                if (constructor == null) return null;
                Debug.WriteLine("Found Startup constructor...");

                var hasConfigurationArg = constructor.GetParameters().Any(p => p.ParameterType == typeof(IConfiguration));
                var hasHostEnvironmentArg = constructor.GetParameters().Any(p => p.ParameterType == typeof(IWebAssemblyHostEnvironment));
                var args = new List<object>();

                if (hasConfigurationArg)
                {
                    Debug.WriteLine("Injecting Configuration arg");
                    args.Add(_builder.Configuration.Build());
                }

                if (hasHostEnvironmentArg)
                {
                    Debug.WriteLine("Injecting HostEnvironment arg");
                    args.Add(_builder.HostEnvironment);
                }

                try
                {
                    return Activator.CreateInstance(startupClassType, args.ToArray());
                }
                catch (Exception)
                {
                    args.Reverse();
                    return Activator.CreateInstance(startupClassType, args.ToArray());
                }
            }

            private void RunStartupConfigureServices(object startupClass, Type startupClassType)
            {
                Debug.WriteLine("Running ConfigureServices method...");
                var configureServicesMethod = startupClassType.GetMethods().FirstOrDefault(m => m.Name.ToLower() == "configureservices");
                if (configureServicesMethod == null) return;
                Debug.WriteLine("Found ConfigureServices method...");

                var hasServicesArg = configureServicesMethod.GetParameters().Any(p => p.ParameterType == typeof(IServiceCollection));
                var hasOneArg = configureServicesMethod.GetParameters().Length == 1;

                if (hasOneArg && hasServicesArg)
                {
                    Debug.WriteLine("Invoking ConfigureServices method...");
                    configureServicesMethod.Invoke(startupClass, new object[] { _builder.Services });
                }
            }

            public class WebAssemblyHostConfigurationBuilderOptions
            {
                public void UseStartup<TStartupClass>()
                {
                    _startupClassType = typeof(TStartupClass);
                }
            }
        }

    }
}