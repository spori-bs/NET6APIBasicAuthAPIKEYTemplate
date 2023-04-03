using FastEndpoints;

using MediatR;

using Net6APIShared.API.Composite;

namespace Net6APIBasicAuthApiKey.Endpoints;

public class CompositeEndpoint : Endpoint<MyCompositeRequest, MyCompositeResponse>
{

    public MediatR.IMediator Mediator { get; set; } = null!;
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/composite");
    }
    public override async Task HandleAsync(MyCompositeRequest request, CancellationToken ct)
    {
        var response = await Mediator.Send(request);
        await SendAsync(response,200,ct);
    }
}
