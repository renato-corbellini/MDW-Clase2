using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Clase2.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddValidatorsFromAssembly(assembly);
        services.AddLocalization();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assembly);
            cfg.AddOpenBehavior(typeof(Common.ValidationBehavior<,>));
        });

        return services;
    }
}