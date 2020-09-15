using IdentityModel;

namespace IdentityServer.CustomIdentityResources
{
    public class ProfileWithRoleIdentityResource : IdentityServer4.Models.IdentityResources.Profile
    {
        public ProfileWithRoleIdentityResource()
        {
            this.UserClaims.Add(JwtClaimTypes.Role);
        }
    }
}