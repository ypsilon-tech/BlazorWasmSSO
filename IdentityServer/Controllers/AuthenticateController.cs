using System;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Models;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;

        public AuthenticateController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IIdentityServerInteractionService interaction, 
            IEventService events)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _events = events;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            
            var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);
            string redirectUrl;
            string loginError = string.Empty;

            if (request.RequestAction != "login")
            {

                if (context != null)
                {
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
                    redirectUrl = request.ReturnUrl;
                }
                else
                {
                    redirectUrl = string.Empty;
                }

                return new JsonResult(new
                {
                    redirectTo = redirectUrl
                });
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, true);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(request.Username);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                    if (context != null)
                    {
                        redirectUrl = request.ReturnUrl;
                    }
                    else if (Url.IsLocalUrl(request.ReturnUrl))
                    {
                        redirectUrl = request.ReturnUrl;
                    }
                    else if (string.IsNullOrEmpty(request.ReturnUrl))
                    {
                        redirectUrl = string.Empty;
                    }
                    else
                    {
                        throw new Exception("invalid return URL");
                    }

                    return new JsonResult(new
                    {
                        redirectTo = redirectUrl
                    });
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(request.Username, "invalid credentials", clientId: context?.Client.ClientId));
                loginError = "Invalid username or password";
            }

            return new JsonResult(new
            {
                error = !string.IsNullOrWhiteSpace(loginError) ? loginError : "An unexpected error was encountered during login, please try again"
            });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            var showSignoutPrompt = context?.ShowSignoutPrompt != false;
            string externalAuth = null;

            if (User?.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout) externalAuth = idp;
                }
            }

            return Ok(new
            {
                showSignoutPrompt,
                ClientName = string.IsNullOrEmpty(context?.ClientName) ? context?.ClientId : context.ClientName,
                postLogoutRedirectUri = context?.PostLogoutRedirectUri,
                signOutIFrameUrl = context?.SignOutIFrameUrl,
                logoutId,
                externalAuth
            });
        }
    }
}
