using System.Buffers;
using System.Text;
using System.Text.Json;

namespace SchemaRender;

/// <summary>
/// Renders schemas as JSON-LD script tags for inclusion in HTML documents.
/// Uses Utf8JsonWriter for high-performance, zero-allocation JSON generation.
/// </summary>
public static class SchemaRenderer
{
    private static readonly byte[] ScriptOpen = "<script type=\"application/ld+json\">"u8.ToArray();
    private static readonly byte[] ScriptClose = "</script>"u8.ToArray();

    private static readonly JsonWriterOptions WriterOptions = new()
    {
        Indented = false,
        SkipValidation = true // We control the output, no need for validation overhead
    };

    /// <summary>
    /// Renders all schemas from the context as JSON-LD script tags to the specified stream.
    /// Each schema is wrapped in its own script tag.
    /// </summary>
    /// <param name="context">The schema context containing schemas to render.</param>
    /// <param name="stream">The output stream to write to.</param>
    public static void Render(ISchemaContext context, Stream stream)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(stream);

        if (!context.HasSchemas)
            return;

        foreach (var schema in context.Schemas)
        {
            RenderSchema(schema, stream);
        }
    }

    /// <summary>
    /// Renders a single schema as a JSON-LD script tag to the specified stream.
    /// </summary>
    /// <param name="schema">The schema to render.</param>
    /// <param name="stream">The output stream to write to.</param>
    public static void RenderSchema(ISchema schema, Stream stream)
    {
        ArgumentNullException.ThrowIfNull(schema);
        ArgumentNullException.ThrowIfNull(stream);

        stream.Write(ScriptOpen);

        using var writer = new Utf8JsonWriter(stream, WriterOptions);
        schema.Write(writer);
        writer.Flush();

        stream.Write(ScriptClose);
    }

    /// <summary>
    /// Renders all schemas from the context as JSON-LD script tags and returns the result as a string.
    /// Use this method for Razor integration where IHtmlContent is needed.
    /// </summary>
    /// <param name="context">The schema context containing schemas to render.</param>
    /// <returns>The rendered JSON-LD script tags as a string, or empty string if no schemas.</returns>
    public static string RenderToString(ISchemaContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (!context.HasSchemas)
            return string.Empty;

        var buffer = new ArrayBufferWriter<byte>(256);

        foreach (var schema in context.Schemas)
        {
            RenderSchemaToBuffer(schema, buffer);
        }

        return Encoding.UTF8.GetString(buffer.WrittenSpan);
    }

    /// <summary>
    /// Renders a single schema as a JSON-LD script tag and returns the result as a string.
    /// </summary>
    /// <param name="schema">The schema to render.</param>
    /// <returns>The rendered JSON-LD script tag as a string.</returns>
    public static string RenderSchemaToString(ISchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);

        var buffer = new ArrayBufferWriter<byte>(256);
        RenderSchemaToBuffer(schema, buffer);
        return Encoding.UTF8.GetString(buffer.WrittenSpan);
    }

    private static void RenderSchemaToBuffer(ISchema schema, ArrayBufferWriter<byte> buffer)
    {
        buffer.Write(ScriptOpen);

        using var writer = new Utf8JsonWriter(buffer, WriterOptions);
        schema.Write(writer);
        writer.Flush();

        buffer.Write(ScriptClose);
    }
}
