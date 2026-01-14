using Microsoft.Extensions.DependencyInjection;

namespace SchemaRender;

/// <summary>
/// Extension methods for registering SchemaRender services with dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds SchemaRender services to the specified service collection.
    /// Registers <see cref="ISchemaContext"/> as a scoped service.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSchemaRender(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<ISchemaContext, SchemaContext>();

        return services;
    }
}
