namespace Net6APIBasicAuthApiKey.Models;

public class ServiceAccessInfo
{
    public string ApiKey { get; set; } = null!;
    public string User { get; set; } = null!;
    public string Password { get; set; } = null!;
}