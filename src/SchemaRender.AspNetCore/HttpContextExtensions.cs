using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SchemaRender;

/// <summary>
/// Extension methods for <see cref="HttpContext"/> to access schema functionality.
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Gets the schema context for the current request.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>The schema context.</returns>
    public static ISchemaContext GetSchemaContext(this HttpContext httpContext)
    {
        return httpContext.RequestServices.GetRequiredService<ISchemaContext>();
    }

    /// <summary>
    /// Adds a schema to be rendered for the current request.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="schema">The schema to add.</param>
    public static void AddSchema(this HttpContext httpContext, ISchema schema)
    {
        httpContext.GetSchemaContext().Add(schema);
    }
}
