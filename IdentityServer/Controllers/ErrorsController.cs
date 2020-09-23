using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ErrorsController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IWebHostEnvironment _environment;

        public ErrorsController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment)
        {
            _interaction = interaction;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetError(string errorId)
        {
            var error = await _interaction.GetErrorContextAsync(errorId);

            return new JsonResult(new 
            {
                error = error.Error,
                errorDescription = _environment.IsDevelopment() ? error.ErrorDescription : null,
                requestId = error.RequestId
            });
        }
    }
}