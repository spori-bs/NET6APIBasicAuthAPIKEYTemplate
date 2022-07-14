using Microsoft.AspNetCore.Authentication;
using Net6APIBasicAuthApiKey.Models;

namespace Net6APIBasicAuthApiKey.Auth;

public class BasicAuthenticationOptions : AuthenticationSchemeOptions
{
    public ServiceAccessInfo? ServiceAccessInfo { get; set; }
}