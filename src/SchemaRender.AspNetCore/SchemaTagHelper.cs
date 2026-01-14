using System.Buffers;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SchemaRender;

/// <summary>
/// Tag helper that renders all collected schemas as JSON-LD script tags.
/// Use in layout: &lt;schema-render /&gt;
/// </summary>
[HtmlTargetElement("schema-render", TagStructure = TagStructure.WithoutEndTag)]
public sealed class SchemaTagHelper : TagHelper
{
    private static readonly byte[] ScriptOpen = "<script type=\"application/ld+json\">"u8.ToArray();
    private static readonly byte[] ScriptClose = "</script>"u8.ToArray();

    private static readonly JsonWriterOptions WriterOptions = new()
    {
        Indented = false,
        SkipValidation = true
    };

    private readonly ISchemaContext schemaContext;

    public SchemaTagHelper(ISchemaContext context)
    {
        this.schemaContext = context;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null; // Remove the <schema-render> tag itself

        if (!this.schemaContext.HasSchemas)
        {
            output.SuppressOutput();
            return;
        }

        var buffer = new ArrayBufferWriter<byte>(256);

        foreach (var schema in this.schemaContext.Schemas)
        {
            buffer.Write(ScriptOpen);

            using var writer = new Utf8JsonWriter(buffer, WriterOptions);
            schema.Write(writer);
            writer.Flush();

            buffer.Write(ScriptClose);
        }

        output.Content.SetHtmlContent(System.Text.Encoding.UTF8.GetString(buffer.WrittenSpan));
    }
}
