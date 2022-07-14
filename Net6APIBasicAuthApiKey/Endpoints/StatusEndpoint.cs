using FastEndpoints;

namespace Net6APIBasicAuthApiKey.Endpoints;

public class StatusEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/getstatus");
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendNoContentAsync(ct);
    }
}