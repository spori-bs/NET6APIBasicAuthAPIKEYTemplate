using Net6APIShared.API.Composite;

namespace Net6APILogic.RequestHandlers;
public class CompositeHandler : MediatR.IRequestHandler<MyCompositeRequest, MyCompositeResponse>
{
    public async Task<MyCompositeResponse> Handle(MyCompositeRequest request, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            throw new TaskCanceledException();
        }

        // Modify some data, or do somethting here.

        return await Task.FromResult(new MyCompositeResponse() { MyProperty = request.MyFirstProperty + request.MySecondProperty });
    }
}


