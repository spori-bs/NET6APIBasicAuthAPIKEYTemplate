using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Net6APIBasicAuthApiKey.Models;

namespace Net6APIBasicAuthApiKey.Auth;

public class AuthorizationService : IAuthorizationService
{
    private readonly ILogger<AuthorizationService> _logger;
        private readonly ServiceAccessInfo _serviceAccessInfo;
        internal const string ApiKeyHeaderValue = "API-KEY";
        public AuthorizationService(ILogger<AuthorizationService> logger, IOptions<ServiceAccessInfo> options)
        {
            _logger = logger;
            _serviceAccessInfo = options.Value;
        }
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            if (resource is not HttpContext httpContext || user.Identity is null) 
            {
                return Task.FromResult(AuthorizationResult.Failed(AuthorizationFailure.Failed(requirements)));
            }

            var ip = httpContext.Connection.RemoteIpAddress;

            if (!user.Identity.IsAuthenticated)
            {
                _logger.LogInformation("Unauthorized access {ip}", ip);
                return Task.FromResult(AuthorizationResult.Failed(AuthorizationFailure.Failed(requirements)));
            }

            if (!httpContext.Request.Headers.ContainsKey(ApiKeyHeaderValue))
            {
                _logger.LogInformation("Missing API-KEY:{ip}", ip);
                return Task.FromResult(AuthorizationResult.Failed(AuthorizationFailure.Failed(requirements)));
            }

            var headerApiKey = httpContext.Request.Headers[ApiKeyHeaderValue];
            if (_serviceAccessInfo.ApiKey != headerApiKey)
            {
                _logger.LogInformation("Invalid API-KEY:{key}:{ip}", headerApiKey, ip);
                return Task.FromResult(AuthorizationResult.Failed(AuthorizationFailure.Failed(requirements)));
            }
            return Task.FromResult(AuthorizationResult.Success());
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        {
            throw new NotImplementedException();
        }
}