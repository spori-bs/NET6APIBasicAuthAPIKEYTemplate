using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Net6APIBasicAuthApiKey.Helpers;

namespace Net6APIBasicAuthApiKey.Auth;

public class AuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
{
    internal const string BasicAuthenticationSchemeName = "Basic";

    public AuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {

        if (!Request.Headers.ContainsKey(Microsoft.Net.Http.Headers.HeaderNames.Authorization))
        {
            Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.WWWAuthenticate, BasicAuthenticationSchemeName);
            AuthenticateResult result =
                AuthenticateResult.Fail("Proper authentication information is necessary to using this app.");
            return Task.FromResult(result);
        }

        if (Options.ServiceAccessInfo is null)
        {
            base.Logger.LogError("Error in Authentiacation middleware. {ServiceAccessInfo} is null",
                nameof(Options.ServiceAccessInfo));
            throw new InvalidOperationException("Invalid program state. The ServiceAccessInfo must be valid!");
        }

        var authHeader = Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization];
        var userdata = authHeader.ToString().Remove(0, BasicAuthenticationSchemeName.Length + 1);
        var decodedUserdata = EncodingHelper.Base64Decode(userdata);
        if (decodedUserdata != $"{Options.ServiceAccessInfo.User}:{Options.ServiceAccessInfo.Password}")
        {
            Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.WWWAuthenticate, BasicAuthenticationSchemeName);
            AuthenticateResult result =
                AuthenticateResult.Fail("Proper authentication information is necessary to using this app.");
            return Task.FromResult(result);
        }

        var claims = new[] {new Claim(ClaimTypes.UserData, Options.ServiceAccessInfo.User)};
        var identity = new ClaimsIdentity(claims, nameof(AuthenticationHandler));
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), this.Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}