using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SchemaRender;

/// <summary>
/// Base class for MVC Controllers that provides easy access to the schema context.
/// Inherit from this class to use Schema.Add() directly in your controllers.
/// </summary>
public abstract class SchemaController : Controller
{
    private ISchemaContext? schema;

    /// <summary>
    /// Gets the schema context for adding structured data to the response.
    /// </summary>
    protected ISchemaContext Schema => this.schema ??= HttpContext.RequestServices.GetRequiredService<ISchemaContext>();
}
