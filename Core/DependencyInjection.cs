namespace Core;

using System.Reflection;
using Common.Behaviour;
using Core.Customer.Commands;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<UpdateApplicationUserCommand>());
        AssemblyScanner.FindValidatorsInAssembly(typeof(CreatePizzaCommand).Assembly)
           .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}