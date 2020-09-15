using IdentityServer.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityServer.Configuration
{
    public static class ConfigurationExtensions
    {
        public static ConnectionStrings GetConnectionStrings(this IConfiguration configuration)
        {
            var connectionStrings = new ConnectionStrings();
            configuration.GetSection("ConnectionStrings").Bind(connectionStrings);
            return connectionStrings;
        }
    }
}