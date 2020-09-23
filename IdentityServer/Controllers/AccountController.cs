using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = request.Username, Email = request.EmailAddress};
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Name, request.Name));
                    await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Email, request.EmailAddress));
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Created("Account", new {username = request.Username});
                }

            }

            return BadRequest();
        }
    }
}