using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginProvidersController : ControllerBase
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;

        public LoginProvidersController(IAuthenticationSchemeProvider schemeProvider, IIdentityServerInteractionService interaction, IClientStore clientStore)
        {
            _schemeProvider = schemeProvider;
            _interaction = interaction;
            _clientStore = clientStore;
        }

        [HttpGet("external")]
        public async Task<IActionResult> GetExternalProviders(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(s => s.DisplayName != null)
                .Select(s => new ExternalProvider
                {
                    Scheme = s.Name,
                    DisplayName = s.DisplayName ?? s.Name
                })
                .ToList();

            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindClientByIdAsync(context.Client.ClientId);

                if (client?.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                {
                    providers = providers
                        .Where(provider => client.IdentityProviderRestrictions.Contains(provider.Scheme)).ToList();
                }
            }

            return new JsonResult(providers);
        }

        
    }
}