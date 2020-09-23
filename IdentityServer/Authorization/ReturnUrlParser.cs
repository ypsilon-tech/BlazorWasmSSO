using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityServer.Authorization
{
    public class ReturnUrlParser : IReturnUrlParser
    {
        public static class ProtocolRoutePaths
        {
            public const string Authorize = "connect/authorize";
            public const string AuthorizeCallback = Authorize + "/callback";
        }

        private readonly IAuthorizeRequestValidator _validator;
        private readonly IUserSession _userSession;
        private readonly IHttpContextAccessor _contextAccessor;

        public ReturnUrlParser(IAuthorizeRequestValidator validator, IUserSession userSession, IHttpContextAccessor contextAccessor)
        {
            _validator = validator;
            _userSession = userSession;
            _contextAccessor = contextAccessor;
        }

        public async Task<AuthorizationRequest> ParseAsync(string returnUrl)
        {
            if (IsValidReturnUrl(returnUrl))
            {
                var parameters = returnUrl.ReadQueryStringAsNameValueCollection();
                var user = await _userSession.GetUserAsync();
                var result = await _validator.ValidateAsync(parameters, user);

                if (!result.IsError)
                {
                    return result.ValidatedRequest.ToAuthorizatonRequest();
                }
            }

            return null;
        }

        public bool IsValidReturnUrl(string returnUrl)
        {
            var hostOrigin = $"{_contextAccessor.HttpContext.Request.Scheme}://{ _contextAccessor.HttpContext.Request.Host}";

            if (returnUrl.IsLocalUrl() || returnUrl.StartsWith(hostOrigin))
            {
                var index = returnUrl.IndexOf('?');

                if (index >= 0)
                {
                    returnUrl = returnUrl.Substring(0, index);
                }

                if (returnUrl.EndsWith(ProtocolRoutePaths.Authorize, StringComparison.OrdinalIgnoreCase) ||
                    returnUrl.EndsWith(ProtocolRoutePaths.AuthorizeCallback, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }

    internal static class Extensions
    {
        public static NameValueCollection ReadQueryStringAsNameValueCollection(this string url)
        {
            if (url != null)
            {
                var idx = url.IndexOf("?");

                if (idx >= 0)
                {
                    url = url.Substring(idx + 1);
                }

                var query = QueryHelpers.ParseNullableQuery(url);

                if (query != null)
                {
                    return query.AsNameValueCollection();
                }
            }

            return new NameValueCollection();
        }

        public static AuthorizationRequest ToAuthorizatonRequest(this ValidatedAuthorizeRequest request)
        {
            var authRequest = new AuthorizationRequest
            {
                Client = request.Client,
                RedirectUri = request.RedirectUri,
                DisplayMode = request.DisplayMode,
                UiLocales = request.UiLocales,
                IdP = request.GetIdP(),
                Tenant = request.GetTenant(),
                LoginHint = request.LoginHint,
                PromptModes = request.PromptModes,
                AcrValues = request.GetAcrValues(),
                ValidatedResources = request.ValidatedResources
            };

            authRequest.Parameters.Add(request.Raw);
            request.RequestObjectValues.Keys.ToList().ForEach(key => authRequest.RequestObjectValues.Add(key, request.RequestObjectValues[key]));

            return authRequest;
        }

        public static bool IsLocalUrl(this string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            if (url[0] == '/')
            {
                if (url.Length == 1)
                {
                    return true;
                }

                if (url[1] != '/' && url[1] != '\\')
                {
                    return true;
                }

                return false;
            }

            if (url[0] == '~' && url.Length > 1 && url[1] == '/')
            {
                if (url.Length == 2)
                {
                    return true;
                }

                if (url[2] != '/' && url[2] != '\\')
                {
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}