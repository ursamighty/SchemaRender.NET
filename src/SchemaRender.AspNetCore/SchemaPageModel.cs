using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace SchemaRender;

/// <summary>
/// Base class for Razor Pages that provides easy access to the schema context.
/// Inherit from this class to use Schema.Add() directly in your page models.
/// </summary>
public abstract class SchemaPageModel : PageModel
{
    private ISchemaContext? schema;

    /// <summary>
    /// Gets the schema context for adding structured data to the page.
    /// </summary>
    protected ISchemaContext Schema => this.schema ??= HttpContext.RequestServices.GetRequiredService<ISchemaContext>();
}
