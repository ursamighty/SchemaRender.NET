using System.Buffers;
using System.Text.Json;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace SchemaRender;

/// <summary>
/// Extension methods for <see cref="IHtmlHelper"/> to render schemas.
/// </summary>
public static class HtmlHelperExtensions
{
    private static readonly byte[] ScriptOpen = "<script type=\"application/ld+json\">"u8.ToArray();
    private static readonly byte[] ScriptClose = "</script>"u8.ToArray();

    private static readonly JsonWriterOptions WriterOptions = new()
    {
        Indented = false,
        SkipValidation = true
    };

    /// <summary>
    /// Renders all collected schemas as JSON-LD script tags.
    /// Use in layout: @Html.RenderSchemas()
    /// </summary>
    /// <param name="html">The HTML helper.</param>
    /// <returns>The rendered JSON-LD script tags as HTML content.</returns>
    public static IHtmlContent RenderSchemas(this IHtmlHelper html)
    {
        var context = html.ViewContext.HttpContext.RequestServices.GetRequiredService<ISchemaContext>();

        if (!context.HasSchemas)
            return HtmlString.Empty;

        var buffer = new ArrayBufferWriter<byte>(256);

        foreach (var schema in context.Schemas)
        {
            buffer.Write(ScriptOpen);

            using var writer = new Utf8JsonWriter(buffer, WriterOptions);
            schema.Write(writer);
            writer.Flush();

            buffer.Write(ScriptClose);
        }

        return new HtmlString(System.Text.Encoding.UTF8.GetString(buffer.WrittenSpan));
    }

    /// <summary>
    /// Gets the schema context for adding schemas from a view.
    /// Use in pages: @Html.Schema().Add(new RecipeSchema { ... })
    /// </summary>
    /// <param name="html">The HTML helper.</param>
    /// <returns>The schema context.</returns>
    public static ISchemaContext Schema(this IHtmlHelper html)
    {
        return html.ViewContext.HttpContext.RequestServices.GetRequiredService<ISchemaContext>();
    }
}
