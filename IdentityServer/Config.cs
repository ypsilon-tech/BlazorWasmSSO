// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer.CustomIdentityResources;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(
                    "weatherapi",
                    "The Weather API",
                    userClaims: new[] {JwtClaimTypes.Role})
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "blazor",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    AllowedCorsOrigins = {"https://localhost:5002"},
                    RedirectUris = {"https://localhost:5002/authentication/login-callback"},
                    PostLogoutRedirectUris = {"https://localhost:5002/"},

                    AllowedScopes = {"openid", "profile", "email", "weatherapi"},

                    Enabled = true
                },
                new Client
                {
                    ClientId = "angular",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    AllowedCorsOrigins = {"https://localhost:5004"},
                    RedirectUris = {"https://localhost:5004/auth-callback"},
                    PostLogoutRedirectUris = {"https://localhost:5004/"},

                    AllowedScopes = {"openid", "profile", "email", "weatherapi"},

                    Enabled = true
                }
            };
    }
}