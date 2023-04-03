using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Net6APILogic;
public static class LogicDIRegistrationExtension
{

    public static void APILogicRegistration(this IServiceCollection services)
    {
        services.AddMediatR( x =>
        {
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }
}
