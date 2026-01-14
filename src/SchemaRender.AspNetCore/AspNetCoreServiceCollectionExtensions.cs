using Microsoft.Extensions.DependencyInjection;

namespace SchemaRender;

/// <summary>
/// Extension methods for registering SchemaRender ASP.NET Core services.
/// </summary>
public static class AspNetCoreServiceCollectionExtensions
{
    /// <summary>
    /// Adds SchemaRender services including ASP.NET Core integration to the specified service collection.
    /// Registers <see cref="ISchemaContext"/> as a scoped service and enables Tag Helper support.
    /// </summary>
    /// <remarks>
    /// After calling this method, add the following to your _ViewImports.cshtml:
    /// <code>
    /// @addTagHelper *, SchemaRender.AspNetCore
    /// </code>
    /// </remarks>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSchemaRender(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Register core services
        services.AddScoped<ISchemaContext, SchemaContext>();

        return services;
    }
}
