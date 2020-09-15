﻿using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace BlazorClient.Security
{
    public class ArrayClaimsPrincipalFactory<TAccount> : AccountClaimsPrincipalFactory<TAccount> where TAccount : RemoteUserAccount
    {
        public ArrayClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor) : base(accessor)
        {
        }

        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(TAccount account, RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);
            var claimsIdentity = (ClaimsIdentity)user.Identity;

            if (account != null)
            {
                foreach (var kvp in account.AdditionalProperties)
                {
                    var value = kvp.Value;

                    if (value != null
                        && value is JsonElement element
                        && element.ValueKind == JsonValueKind.Array)
                    {
                        claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(kvp.Key));
                        var claims = element.EnumerateArray()
                            .Select(x => new Claim(kvp.Key, x.ToString()));
                        claimsIdentity.AddClaims(claims);
                    }
                }
            }

            return user;

        }
    }
}
