namespace Net6APIShared.API.Composite;

public class MyCompositeRequest: MediatR.IRequest<MyCompositeResponse>
{
    public int MyFirstProperty { get; set; } = 0;
    public int MySecondProperty { get; set; } = 0;
}
